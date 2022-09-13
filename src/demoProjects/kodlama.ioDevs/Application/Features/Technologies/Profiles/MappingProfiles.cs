using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Technologies.Commands.CreateTechnology;
using Application.Features.Technologies.Commands.DeleteTechnology;
using Application.Features.Technologies.Commands.UpdateTechnology;
using Application.Features.Technologies.Dtos;
using Application.Features.Technologies.Models;
using AutoMapper;
using Core.Persistence.Paging;
using Domain.Entities;

namespace Application.Features.Technologies.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Technology, CreatedTechnologyDto>().ReverseMap();
            CreateMap<Technology, CreateTechnologyCommand>().ReverseMap();

            CreateMap<Technology, DeletedTechnologyDto>().ReverseMap();
            CreateMap<Technology, DeleteTechnologyCommand>().ReverseMap();

            CreateMap<Technology, UpdatedTechnologyDto>().ReverseMap();
            CreateMap<Technology, UpdateTechnologyCommand>().ReverseMap();

            CreateMap<Technology, TechnologyListDto>()
                .ForMember(t => t.LanguageName, opt => opt.MapFrom(t => t.Language.Name))
                .ReverseMap();
            CreateMap<IPaginate<Technology>, TechnologyListModel>().ReverseMap();

            CreateMap<Technology, TechnologyGetByIdDto>()
                .ForMember(t => t.LanguageName, opt => opt.MapFrom(t => t.Language.Name))
                .ReverseMap();
        }
    }
}
