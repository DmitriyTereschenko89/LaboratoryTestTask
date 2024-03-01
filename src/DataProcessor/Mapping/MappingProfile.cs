using AutoMapper;
using DataProcessor.Entities;
using EventBus.Messages.Models;

namespace DataProcessor.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DeviceStatus, ModelEntity>()
            .ForMember(dest => dest.ModuleCategoryID, opt => opt.MapFrom(src => src.ModuleCategoryID.ToString()))
            .ForMember(dest => dest.ModuleState, opt => opt.MapFrom(src => src.RapidControlStatus.ModuleState.ToString()));
    }
}
