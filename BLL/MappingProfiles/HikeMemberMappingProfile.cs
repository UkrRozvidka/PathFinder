using AutoMapper;
using BLL.DTOs.HikeMember;
using DAL.Entities;

namespace BLL.MappingProfiles
{
    public class HikeMemberMappingProfile : Profile
    {
        public HikeMemberMappingProfile() 
        {
            CreateMap<HikeMemberCreateDTO, HikeMember>();
            CreateMap<HikeMember, HikeMemberGetDTO>();
            CreateMap<HikeMember, HikeMemberGetFullDTO>();
        }
    }
}
