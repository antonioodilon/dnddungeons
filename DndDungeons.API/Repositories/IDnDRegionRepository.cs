using DndDungeons.API.Models.Domain;

namespace DndDungeons.API.Repositories
{
    public interface IDnDRegionRepository
    {
        Task<List<DndRegion>> GetAllAsync();
        Task<DndRegion?> GetRegionByIdAsync(Guid id);
        Task<DndRegion> CreateRegionAsync(DndRegion region);
        // We need to return this because the function creates an id for regionDomainModel!
        Task<DndRegion?> UpdateRegionAsync(Guid id, DndRegion region);
        //Task<Region?> DeleteRegionAsync(Guid 

        Task<DndRegion?> DeleteRegionAsync(Guid id);
    }
}
