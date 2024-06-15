using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext _dbContext;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
    {
        _dbContext = dbContext;
        _regionRepository = regionRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

        await _regionRepository.CreatAsync(regionDomainModel);

        var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateRegsionRequestDto updateRegionRequestDto)
    {
        var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
        regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

        var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

        return Ok(regionDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regionDomainModel = await _regionRepository.GetAllAsync();
        var regionDto = _mapper.Map<List<RegionDto>>(regionDomainModel);
        return Ok(regionDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var regionDomainModel = await _regionRepository.GetByIdAsync(id);
        if (regionDomainModel == null) return NotFound();

        var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var regionDomainModel = await _regionRepository.DeleteAsync(id);
        var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
        return Ok(regionDto);
    }
}