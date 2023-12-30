using DndDungeons.API.Models.Domain;
using DndDungeons.API.Data;
using Microsoft.EntityFrameworkCore;
using DndDungeons.API.Repositories;

namespace NZWalks.API.Repositories
{
    public class SQLDungeonRepository : IDungeonRepository
    {
        private readonly DndDungeonsDbContext _dbContext;
        public SQLDungeonRepository(DndDungeonsDbContext dbContextParam)
        {
            _dbContext = dbContextParam;
        }
        async Task<List<Dungeon>> IDungeonRepository.GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            IQueryable<Dungeon> walks = _dbContext.Dungeons.Include("Difficulty").Include("Region").AsQueryable();

            // Filtering:
            if ((string.IsNullOrWhiteSpace(filterOn) == false) && (string.IsNullOrWhiteSpace(filterQuery) == false))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                } else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                } else
                {
                    return null;
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    if (isAscending == true)
                    {
                        walks = walks.OrderBy(x => x.Name);
                    }
                    else
                    {
                        walks = walks.OrderByDescending(x => x.Name);
                    }
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    if (isAscending == true)
                    {
                        walks = walks.OrderBy(x => x.LengthInKm);
                    }
                    else
                    {
                        walks = walks.OrderByDescending(x => x.LengthInKm);
                    }
                }
            }

            // Pagination
            int skipResults = (pageNumber - 1) * pageSize; // If I want the first page with 5 results, then the first 5 results will
            // be returned because skipResults will be 0. If I want the second page with 5 results, then the first 6 results will be
            // skipped and the program will get the next 6.

            //return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        async Task<Dungeon?> IDungeonRepository.GetDungeonByIdAsync(Guid id)
        {
            //throw new NotImplementedException();
            return await _dbContext.Dungeons.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        async Task<Dungeon> IDungeonRepository.CreateDungeonAsync(Dungeon dungeon)
        {
            //throw new NotImplementedException();
            await _dbContext.Dungeons.AddAsync(dungeon);
            await _dbContext.SaveChangesAsync();
            return dungeon;
        }

        async Task<Dungeon?> IDungeonRepository.UpdateDungeonAsync(Guid dungeonId, Dungeon dungeon)
        {
            //throw new NotImplementedException();
            Dungeon walkToFind = await _dbContext.Dungeons.FirstOrDefaultAsync(x => x.Id == dungeonId);
            if (walkToFind == null)
            {
                return null;
            }

            walkToFind.Name = dungeon.Name;
            walkToFind.Description = dungeon.Description;
            walkToFind.LengthInKm = dungeon.LengthInKm;
            walkToFind.DungeonImageUrl = dungeon.DungeonImageUrl;
            walkToFind.DifficultyId = dungeon.DifficultyId;
            walkToFind.RegionId = dungeon.RegionId;

            bool validDifficulty = false;
            List<Difficulty> listDifficulties = await _dbContext.Difficulties.ToListAsync();
            for (int i = 0; i < listDifficulties.Count; ++i)
            {
                if (listDifficulties[i].Id == walkToFind.DifficultyId)
                {
                    walkToFind.Difficulty = listDifficulties[i];
                    validDifficulty = true;
                    break;
                }
            }

            if (validDifficulty == false)
            {
                return null;
            }

            bool validRegion = false;
            List<DndRegion> listRegions = await _dbContext.Regions.ToListAsync();
            for (int i = 0; i < listRegions.Count; ++i)
            {
                if (listRegions[i].Id == walkToFind.RegionId)
                {
                    walkToFind.Region = listRegions[i];
                    validRegion = true;
                    break;
                }
            }

            if (validRegion == false)
            {
                return null;
            }
            // allow to change Difficulty and Region as well?

            await _dbContext.SaveChangesAsync();
            return walkToFind;
        }

        async Task<Dungeon?> IDungeonRepository.DeleteDungeonAsync(Guid walkId)
        {
            //throw new NotImplementedException();
            Dungeon walkToFind = await _dbContext.Dungeons.FirstOrDefaultAsync(x => x.Id == walkId);
            if (walkToFind == null)
            {
                return null;
            }

            _dbContext.Dungeons.Remove(walkToFind);
            await _dbContext.SaveChangesAsync();

            return walkToFind;
            //await _dbContext.Walks.Remove
        }
    }
}
