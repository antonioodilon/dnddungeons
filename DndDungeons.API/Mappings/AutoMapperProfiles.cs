using AutoMapper;
using DndDungeons.API.Models.Domain;
using DndDungeons.API.Models.DTO;

namespace DndDungeons.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DndRegion, DndRegionDTO>().ReverseMap();
            CreateMap<DndRegion, AddDndRegionRequestDTO>().ReverseMap();
            CreateMap<DndRegion, UpdateDndRegionRequestDTO>().ReverseMap();

            CreateMap<Dungeon, DungeonDTO>().ReverseMap();
            CreateMap<Dungeon, UpdateDungeonRequestDTO>().ReverseMap();

            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
        }
    }
}
