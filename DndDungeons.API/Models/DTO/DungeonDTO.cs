using DndDungeons.API.Models.Domain;

namespace DndDungeons.API.Models.DTO
{
    public class DungeonDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? DungeonImageUrl { get; set; }
        // Now we can remove these because they will be shown in the navigation properties!
        //public Guid DifficultyId { get; set; }
        //public Guid RegionId { get; set; }


        // Navigation properties
        public DifficultyDTO Difficulty { get; set; }
        public DndRegionDTO Region { get; set; }
        //public Difficulty Difficulty { get; set; }
        //public Region Region { get; set; }
    }
}
