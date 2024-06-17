using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings;

public class AutomapperProfiles : Profile
{
    public AutomapperProfiles()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegsionRequestDto, Region>().ReverseMap();
        
        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
        CreateMap<Walk, WalkDto>().ReverseMap();
    }
}