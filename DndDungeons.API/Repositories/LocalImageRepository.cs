using Microsoft.EntityFrameworkCore;
using DndDungeons.API.Data;
using DndDungeons.API.Models.Domain;

namespace DndDungeons.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DndDungeonsDbContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,
            DndDungeonsDbContext dbContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this._dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            string localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            // Upload Image to Local Path:
            FileStream fileStream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(fileStream);

            // https://localhost:1234/images/someimage.jpg -> Since it's the web, then we need a sort of web location
            string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            HostString host = _httpContextAccessor.HttpContext.Request.Host;
            string pathBase = _httpContextAccessor.HttpContext.Request.PathBase;

            string urlFilePath = $"{scheme}://{host}{pathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            try
            {
                // Add Image to the Images table:
                await _dbContext.AddAsync(image);
                //_dbContext.Entry(image).State = EntityState.Added;
                await _dbContext.SaveChangesAsync();
                return image;
            } catch (Exception ex)
            {
                Console.WriteLine($"An error occurred! {ex} -> {ex.Message}");
                throw;
            }

            //return image;
        }
    }
}
