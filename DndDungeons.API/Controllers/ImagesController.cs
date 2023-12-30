using DndDungeons.API.Models.DTO;
using DndDungeons.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DndDungeons.API.Models.Domain;

namespace DndDungeons.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO imageRequest)
        {
            ValidateFileUpload(imageRequest);

            if (ModelState.IsValid)
            {
                // Convert DTO to Domain:
                Image validImage = new Image();
                //validImage.Id = Guid.NewGuid();
                validImage.File = imageRequest.File;
                validImage.FileName = imageRequest.FileName;
                validImage.FileDescription = imageRequest.FileDescription;
                validImage.FileExtension = Path.GetExtension(imageRequest.File.FileName);
                validImage.FileSizeInBytes = imageRequest.File.Length;

                // User repository to upload image
                await _imageRepository.Upload(validImage);

                ImageResponseDTO imageResponse = new ImageResponseDTO
                {
                    Id = validImage.Id,
                    File = validImage.File,
                    FileName = validImage.FileName,
                    FileExtension = validImage.FileExtension,
                    FileDescription = validImage.FileDescription,
                    FileSizeInBytes = validImage.FileSizeInBytes,
                    FilePath = validImage.FilePath,
                };

                return Ok(imageResponse);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO imageRequest)
        {
            string[] allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };
            long maximumSizeInBytes = 10485760;

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                //if (imageRequest.File.FileName.Contains(allowedExtensions[i]))
                    //break;

                //if (!(imageRequest.File.FileName.Contains(allowedExtensions[i])) && (i == allowedExtensions.Length - 1))
                if (!(imageRequest.File.FileName.Contains(allowedExtensions[i])))
                {
                    if (i == allowedExtensions.Length - 1)
                    {
                        ModelState.AddModelError("file", "Unsupported file extension.");
                        return;
                    }
                }
                return;
            }

            /*if (allowedExtensions.Contains(Path.GetExtension(imageRequest.File.FileName)))
            {

            }*/

            if (imageRequest.File.Length > maximumSizeInBytes)
            {
                ModelState.AddModelError("file", "File size is more than 10MB. Please upload a smaller size file.");
                return;
            }
        }
    }
}
