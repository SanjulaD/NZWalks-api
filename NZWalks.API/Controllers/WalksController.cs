using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly IMapper _mapper;
    public readonly IWalkRepository _walkRepository;

    public WalksController(IMapper mapper, IWalkRepository walkRepository)
    {
        _walkRepository = walkRepository;
        _mapper = mapper;
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

        await _walkRepository.CreateAsync(walkDomainModel);

        var regionDto = _mapper.Map<WalkDto>(walkDomainModel);

        return Ok(regionDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 1000)
    {
        var walksDomainModel =
            await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

        var walkDto = _mapper.Map<List<WalkDto>>(walksDomainModel);
        return Ok(walkDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walksDomainModel = await _walkRepository.GetByIdAsync(id);
        if (walksDomainModel == null) return NotFound();

        var walksDto = _mapper.Map<WalkDto>(walksDomainModel);
        return Ok(walksDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalksRequestDto updateWalksRequestDto)
    {
        var walksDomainModel = _mapper.Map<Walk>(updateWalksRequestDto);
        walksDomainModel = await _walkRepository.UpdateAsync(id, walksDomainModel);

        if (walksDomainModel == null) return NotFound();

        var walkDto = _mapper.Map<WalkDto>(walksDomainModel);

        return Ok(walkDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var walksDomainModel = await _walkRepository.DeleteAsync(id);
        var walksDto = _mapper.Map<WalkDto>(walksDomainModel);
        return Ok(walksDto);
    }
}