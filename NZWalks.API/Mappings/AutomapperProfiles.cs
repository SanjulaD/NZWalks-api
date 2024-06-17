using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Difficulty = NZWalks.API.Models.Domain.Difficulty;

namespace NZWalks.API.Mappings;

public class AutomapperProfiles : Profile
{
    public AutomapperProfiles()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

        CreateMap<Walk, WalkDto>().ReverseMap();
        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
        CreateMap<UpdateWalksRequestDto, Walk>().ReverseMap();

        CreateMap<Difficulty, DifficultyDto>().ReverseMap();
    }
}