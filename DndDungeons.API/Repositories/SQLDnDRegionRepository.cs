using DndDungeons.API.Models.Domain;
using DndDungeons.API.Data;
using Microsoft.EntityFrameworkCore;
using DndDungeons.API.Repositories;

namespace NZWalks.API.Repositories
{
    public class SQLDnDRegionRepository : IDnDRegionRepository
    {
        private readonly DndDungeonsDbContext _dbContext;
        public SQLDnDRegionRepository(DndDungeonsDbContext dbContextParam)
        {
            _dbContext = dbContextParam;
        }

        async Task<List<DndRegion>> IDnDRegionRepository.GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
            //throw new NotImplementedException();
        }

        public async Task<DndRegion?> GetRegionByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            //throw new NotImplementedException();
        }

        // We need to return this because the function creates an id for regionDomainModel!
        public async Task<DndRegion> CreateRegionAsync(DndRegion region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
            //throw new NotImplementedException();
        }

        public async Task<DndRegion?> UpdateRegionAsync(Guid id, DndRegion region)
        {
            DndRegion regionToFind = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionToFind == null)
            {
                return null;
            }

            regionToFind.Code = region.Code;
            regionToFind.Name = region.Name;
            regionToFind.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return regionToFind;
            //throw new NotImplementedException();
        }

        public async Task<DndRegion?> DeleteRegionAsync(Guid id)
        {
            DndRegion regionToFind = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionToFind == null)
            {
                return null;
            }

            _dbContext.Regions.Remove(regionToFind);
            await _dbContext.SaveChangesAsync();

            return regionToFind;
        }
    }
}
