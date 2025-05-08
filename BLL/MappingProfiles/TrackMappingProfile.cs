using AutoMapper;
using BLL.DTOs.Track;
using DAL.Entities;

namespace BLL.MappingProfiles
{
    public class TrackMappingProfile : Profile
    {
        public TrackMappingProfile() 
        {
            CreateMap<TrackCreateDTO, Track>();
            CreateMap<Track, TrackGetDTO>();
        }
    }
}
