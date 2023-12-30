using Microsoft.AspNetCore.Mvc;
using DndDungeons.API.Models.Domain;

namespace DndDungeons.API.Repositories
{
    public interface IDungeonRepository
    {
        Task<List<Dungeon>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Dungeon?> GetDungeonByIdAsync(Guid id);
        Task<Dungeon> CreateDungeonAsync(Dungeon dungeon);
        Task<Dungeon?> UpdateDungeonAsync(Guid dungeonId, Dungeon dungeon);
        Task<Dungeon?> DeleteDungeonAsync(Guid dungeonId);
    }
}
