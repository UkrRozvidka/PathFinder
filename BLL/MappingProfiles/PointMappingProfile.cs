using AutoMapper;
using BLL.DTOs.Point;
using DAL.Entities;


namespace BLL.MappingProfiles
{
    public class PointMappingProfile : Profile
    {
        public PointMappingProfile() 
        {
            CreateMap<PointCreateDTO, Point>()
                .ForMember(dest => dest.GeoPoint, opt => opt.MapFrom(src => src.GeoPoint));
            CreateMap<PointUpdateDTO, Point>();
            CreateMap<Point, PointGetDTO>()
                .ForMember(dest => dest.GeoPoint, opt => opt.MapFrom(src => src.GeoPoint));
        }
    }
}
