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

    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
    {
        _dbContext = dbContext;
        _regionRepository = regionRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = new Region
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        await _dbContext.AddAsync(regionDomainModel);
        await _dbContext.SaveChangesAsync();

        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            Code = regionDomainModel.Code,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateRegsionRequestDto updateRegionRequestDto)
    {
        var regionDomainModel = await _dbContext.Regions.FindAsync(id);
        if (regionDomainModel == null) return NotFound();

        regionDomainModel.Name = updateRegionRequestDto.Name;
        regionDomainModel.Code = updateRegionRequestDto.Code;
        regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

        await _dbContext.SaveChangesAsync();

        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            Code = regionDomainModel.Code,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regionDomainModel = await _regionRepository.GetAllAsync();
        var regionDto = new List<RegionDto>();

        foreach (var regionDomain in regionDomainModel)
            regionDto.Add(new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            });
        return Ok(regionDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var regionDomainModel = await _dbContext.Regions.FindAsync(id);
        if (regionDomainModel == null) return NotFound();

        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            Code = regionDomainModel.Code,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var regionDomainModel = await _dbContext.Regions.FindAsync(id);
        if (regionDomainModel == null) return NotFound();

        _dbContext.Regions.Remove(regionDomainModel);
        await _dbContext.SaveChangesAsync();

        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            Code = regionDomainModel.Code,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };

        return Ok(regionDto);
    }
}