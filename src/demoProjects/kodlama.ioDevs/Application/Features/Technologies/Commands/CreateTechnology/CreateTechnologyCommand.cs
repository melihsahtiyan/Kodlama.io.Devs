using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Technologies.Dtos;
using Application.Features.Technologies.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Technologies.Commands.CreateTechnology
{
    public class CreateTechnologyCommand : IRequest<CreatedTechnologyDto>
    {
        public string Name { get; set; }
        public int LanguageId { get; set; }

        public class CreateTechnologyCommandHandler : IRequestHandler<CreateTechnologyCommand, CreatedTechnologyDto>
        {
            private readonly ITechnologyRepository _technologyRepository;
            private readonly IMapper _mapper;
            private readonly TechnologyBusinessRules _businessRules;

            public CreateTechnologyCommandHandler(ITechnologyRepository technologyRepository, IMapper mapper, TechnologyBusinessRules businessRules)
            {
                _technologyRepository = technologyRepository;
                _mapper = mapper;
                _businessRules = businessRules;
            }

            public async Task<CreatedTechnologyDto> Handle(CreateTechnologyCommand request, CancellationToken cancellationToken)
            {

                Technology technology = _mapper.Map<Technology>(request);

                await _businessRules.TechnologyNameCanNotBeDuplicatedWhenRequested(technology.Name);
                await _businessRules.LanguageShouldExistWhenTechnologyInserted(technology.LanguageId);

                Technology createdTechnology = await _technologyRepository.AddAsync(technology);
                CreatedTechnologyDto createdTechnologyDto = _mapper.Map<CreatedTechnologyDto>(createdTechnology);

                return createdTechnologyDto;
            }
        }
    }
}
