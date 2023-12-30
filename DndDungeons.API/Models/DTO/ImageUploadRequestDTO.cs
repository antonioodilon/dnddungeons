using System.ComponentModel.DataAnnotations;

namespace DndDungeons.API.Models.DTO
{
    public class ImageUploadRequestDTO
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        //public string FileExtension { get; set; }
        //public long FileSizeInBytes { get; set; }
        //public string FilePath { get; set; }
    }
}
