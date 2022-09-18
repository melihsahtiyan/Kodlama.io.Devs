using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Dtos;
using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;

namespace Application.Features.Users.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserForRegisterDto>().ReverseMap();
            CreateMap<UserForRegisterDto, CreateUserCommand>().ReverseMap();

            CreateMap<TokenDto, AccessToken>().ReverseMap();

            CreateMap<User, UserForLoginDto>().ReverseMap();
            CreateMap<UserForLoginDto, CreateUserCommand>().ReverseMap();
        }
    }
}
