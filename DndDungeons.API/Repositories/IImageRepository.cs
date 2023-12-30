using DndDungeons.API.Models.Domain;

namespace DndDungeons.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
