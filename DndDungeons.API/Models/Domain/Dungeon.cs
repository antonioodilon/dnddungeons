namespace DndDungeons.API.Models.Domain
{
    public class Dungeon
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? DungeonImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }


        // Navigation properties
        public Difficulty Difficulty { get; set; }
        public DndRegion Region { get; set; }
    }
}
