using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext _dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = new Region
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        _dbContext.Add(regionDomainModel);
        _dbContext.SaveChanges();

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
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegsionRequestDto updateRegionRequestDto)
    {
        var regionDomainModel = _dbContext.Regions.Find(id);
        if (regionDomainModel == null) return NotFound();

        regionDomainModel.Name = updateRegionRequestDto.Name;
        regionDomainModel.Code = updateRegionRequestDto.Code;
        regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
        
        _dbContext.SaveChanges();

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
    public IActionResult GetAll()
    {
        var regionDomainModel = _dbContext.Regions.ToList();
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
    public IActionResult GetById([FromRoute] Guid id)
    {
        var regionDomainModel = _dbContext.Regions.Find(id);
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
    public IActionResult DeleteById([FromRoute] Guid id)
    {
        var regionDomainModel = _dbContext.Regions.Find(id);
        if (regionDomainModel == null) return NotFound();

        _dbContext.Regions.Remove(regionDomainModel);
        _dbContext.SaveChanges();
        
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