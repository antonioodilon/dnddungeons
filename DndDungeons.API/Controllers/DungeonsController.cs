using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DndDungeons.API.Data;
using DndDungeons.API.Models.Domain;
using DndDungeons.API.Models.DTO;
using DndDungeons.API.OtherFunctions;
using DndDungeons.API.Repositories;
using DndDungeons.API.Mappings;
using AutoMapper;


namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class DungeonsController : ControllerBase
    {
        private readonly DndDungeonsDbContext _dbContext;
        private readonly IDungeonRepository _dungeonRepository;
        private readonly IMapper _mapper;

        public DungeonsController(DndDungeonsDbContext dbContext, IDungeonRepository dungeonRepository, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._dungeonRepository = dungeonRepository;
            this._mapper = mapper;
        }

        // GETALL Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Centro&sortBy=Name&isAscending=true&pageNumber=1&pageSize=1000
        // (could be Centro, could be whatever the user wants)
        [HttpGet]
        public async Task<IActionResult> GetAllDungeons([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            if (isAscending == null)
            {
                isAscending = true;
            }

            List<Dungeon> listDungeons = await _dungeonRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            // Map Domain models to DTO:
            List<DungeonDTO> listDungeonsDTO = new List<DungeonDTO>();

            // If any error happens, our global excpetion handler in DndDungeons.API.Middlewares will take care of it (uncomment the line below):
            //throw new Exception("This is an artificial exception to test whether or not the global exception handler is working.");
            // TODO: In the future, implement proper error handling for each of the methods instead of the artificial exception above.

            _mapper.Map(listDungeons, listDungeonsDTO);

            return Ok(listDungeonsDTO);
        }

        // GET Walk
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetSingleDungeon([FromRoute] Guid id)
        {
            Dungeon dungeonToFind = await _dungeonRepository.GetDungeonByIdAsync(id);
            if (dungeonToFind == null)
            {
                return NotFound();
            }

            // Map/Convert Domain model to DTO:
            DungeonDTO dungeonDTO = new DungeonDTO();
            _mapper.Map(dungeonToFind, dungeonDTO);

            return Ok(dungeonDTO);
        }

        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        public async Task<IActionResult> CreateDungeon([FromBody] AddDungeonRequestDTO newDungeonRequestDTO)
        {
            // Map or convert DTO to domain model:
            Dungeon dungeonDomainModel = new Dungeon();
            dungeonDomainModel.Name = newDungeonRequestDTO.Name;
            dungeonDomainModel.Description = newDungeonRequestDTO.Description;
            dungeonDomainModel.LengthInKm = newDungeonRequestDTO.LengthInKm;
            dungeonDomainModel.DungeonImageUrl = newDungeonRequestDTO.DungeonImageUrl;
            dungeonDomainModel.DifficultyId = newDungeonRequestDTO.DifficultyId;
            dungeonDomainModel.RegionId = newDungeonRequestDTO.RegionId;

            // Check if the Difficulty exists:
            List<Difficulty> difficultiesList = _dbContext.Difficulties.ToList();
            bool foundDifficulty = false;
            for (int i = 0; i < difficultiesList.Count; i++)
            {
                if (dungeonDomainModel.DifficultyId == difficultiesList[i].Id)
                {
                    dungeonDomainModel.Difficulty = difficultiesList[i];
                    foundDifficulty = true;
                    break;
                }
            }

            if (foundDifficulty == false)
            {
                return NotFound();
            }

            // Check if the Region exists:
            List<DndRegion> regionsList = _dbContext.Regions.ToList();
            bool foundRegion = false;
            for (int i = 0; i < regionsList.Count; i++)
            {
                if (dungeonDomainModel.RegionId == regionsList[i].Id)
                {
                    dungeonDomainModel.Region = regionsList[i];
                    foundRegion = true;
                    break;
                }
            }

            if (foundRegion == false)
            {
                return NotFound();
            }

            // Add new Dungeon to the database and save changes:
            await _dbContext.Dungeons.AddAsync(dungeonDomainModel);
            _dbContext.SaveChangesAsync();

            // Change to DTO!
            DungeonDTO walkDTOreturn = new DungeonDTO();
            _mapper.Map(dungeonDomainModel, walkDTOreturn);

            string actionNameString = nameof(GetSingleDungeon);
            return CreatedAtAction(actionNameString, new { id = walkDTOreturn.Id}, walkDTOreturn);
        }

        // UPDATE Walks
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateDungeon([FromBody] UpdateDungeonRequestDTO dungeonToUpdateDTO, Guid id)
        {
            Dungeon dungeonToFind = new Dungeon();
            _mapper.Map(dungeonToUpdateDTO, dungeonToFind);

            dungeonToFind = await _dungeonRepository.UpdateDungeonAsync(id, dungeonToFind);
            if (dungeonToFind == null)
            {
                return NotFound();
            }

            DungeonDTO walkDTOreturn = new DungeonDTO();
            _mapper.Map(dungeonToFind, walkDTOreturn);

            return Ok(walkDTOreturn);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteDungeon(Guid id)
        {
            Dungeon dungeonToDelete = await _dungeonRepository.DeleteDungeonAsync(id);

            if (dungeonToDelete == null)
            {
                return NotFound();
            }

            DungeonDTO dungeonDeletedDTOreturn = new DungeonDTO();
            _mapper.Map(dungeonToDelete, dungeonDeletedDTOreturn);

            return Ok(dungeonDeletedDTOreturn);
        }
    }
}
