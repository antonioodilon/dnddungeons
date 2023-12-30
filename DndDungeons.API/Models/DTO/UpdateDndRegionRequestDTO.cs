namespace DndDungeons.API.Models.DTO
{
    public class UpdateDndRegionRequestDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
