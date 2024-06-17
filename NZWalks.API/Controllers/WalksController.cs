using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        public readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

            await _walkRepository.CreateAsync(walkDomainModel);
  
            var regionDto = _mapper.Map<WalkDto>(walkDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = walkDomainModel.Id }, regionDto);
        }
    }
}
