using AutoMapper;
using BLL.DTOs.Hike;
using DAL.Entities;


namespace BLL.MappingProfiles
{
    public class HikeMappingProfile : Profile
    {
        public HikeMappingProfile() 
        {
            CreateMap<HikeCreateDTO, Hike>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.StartPoint))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.EndPoint));
            CreateMap<HikeUpdateDTO, Hike>();
            CreateMap<Hike, HikeGetDTO>()
                .ForMember(dest => dest.StartPoint, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.EndPoint, opt => opt.MapFrom(src => src.End));
            CreateMap<Hike, HikeGetFullDTO>()
                .ForMember(dest => dest.StartPoint, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.EndPoint, opt => opt.MapFrom(src => src.End));
        }
    }
}
