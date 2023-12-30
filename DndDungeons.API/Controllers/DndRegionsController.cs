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
using Microsoft.AspNetCore.Authorization;
using Catel.Data;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/DnDRegions
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Now everything inside this class can only be accessed by an authorized user!
    public class DndRegionsController : ControllerBase
    {
        private readonly DndDungeonsDbContext _dbContext;
        private readonly IDnDRegionRepository _regionRepository;
        private readonly IMapper _mapper; // Interface for Profile type in Automapper
        private readonly ILogger<DndRegionsController> _logger;

        public DndRegionsController(DndDungeonsDbContext dbContextVar, IDnDRegionRepository regionRepositoryVar,
            IMapper mapper, ILogger<DndRegionsController> logger)
        {
            this._dbContext = dbContextVar;
            this._regionRepository = regionRepositoryVar;
            this._mapper = mapper;
            this._logger = logger;
        }

        //IActionResult = The response type! Like 400, 200 etc
        // GET ALL REGIONS
        // GET: https://localhost:1234/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader")] // Temporarily commenting the Authorize attribute
        public async Task<IActionResult> GetAll()
        {
            List<DndRegion> regionsDomain = await _regionRepository.GetAllAsync();

            // Map domain models to DTOs
            List<DndRegionDTO> regionsDTO = new List<DndRegionDTO>();
            _mapper.Map(regionsDomain, regionsDTO);

            // Return DTOs
            return Ok(regionsDTO);
        }

        // GET SINGLE REGION (Get Region By ID)
        // GET: https://localhost:1234/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            DndRegion regionToFind = new DndRegion();
            regionToFind = await _regionRepository.GetRegionByIdAsync(id);

            if (regionToFind == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain model to Region DTO
            DndRegionDTO regionDTOtoReturn = new DndRegionDTO();
            _mapper.Map(regionToFind, regionDTOtoReturn);

            // Return DTO back to client
            return Ok(regionDTOtoReturn);
        }

        // POST To Create new Region
        // POST: https://localhost:1234/api/regions
        [HttpPost]
        //[ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddDndRegionRequestDTO addRegionRequestDTOvar) // In a Post method, we receive a Body from the client
        {
            // Map or convert DTO to Domain Model
            DndRegion regionDomainModel = new DndRegion();
            _mapper.Map(addRegionRequestDTOvar, regionDomainModel);

            regionDomainModel = await _regionRepository.CreateRegionAsync(regionDomainModel);
            // We need to return this because the function creates an id for regionDomainModel!

            // Map Domain model back to DTO in order not to expose the data to the user
            DndRegionDTO regionDTOreturn = new DndRegionDTO();
            _mapper.Map(regionDomainModel, regionDTOreturn);

            string actionNameParam = nameof(GetRegionById);
            return CreatedAtAction(actionNameParam, new { id = regionDTOreturn.Id}, regionDTOreturn);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateDndRegionRequestDTO regionToUpdateDTO)
        {
            // Map DTO to domain model
            DndRegion regionToFind = new DndRegion();
            _mapper.Map(regionToUpdateDTO, regionToFind);

            // Check if region exists
            regionToFind = await _regionRepository.UpdateRegionAsync(id, regionToFind);

            if (regionToFind == null)
            {
                return NotFound();
            }

            DndRegionDTO regionDTOreturn = new DndRegionDTO();
            _mapper.Map(regionToFind, regionDTOreturn);

            return Ok(regionDTOreturn);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            DndRegion regionToFind = await _regionRepository.DeleteRegionAsync(id);

            if (regionToFind == null)
            {
                return NotFound();
            }

            DndRegionDTO regionDTOreturn = new DndRegionDTO();
            _mapper.Map(regionToFind, regionDTOreturn);

            return Ok(regionDTOreturn);
        }
    }
}
