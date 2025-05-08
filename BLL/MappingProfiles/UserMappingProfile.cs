using AutoMapper;
using BLL.DTOs.User;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<AuthDTO, User>();
            CreateMap<User, UserGetDTO>();
        } 
    }
}
