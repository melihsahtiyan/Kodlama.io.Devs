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

namespace Application.Features.Technologies.Queries.GetByIdTechnology
{
    public class GetByIdTechnologyQuery : IRequest<TechnologyGetByIdDto>
    {
        public int Id { get; set; }

        public class GetByIdTechnologyQueryHandler : IRequestHandler<GetByIdTechnologyQuery, TechnologyGetByIdDto>
        {
            private readonly ITechnologyRepository _technologyRepository;
            private readonly IMapper _mapper;
            private readonly TechnologyBusinessRules _businessRules;

            public GetByIdTechnologyQueryHandler(ITechnologyRepository technologyRepository, IMapper mapper, TechnologyBusinessRules businessRules)
            {
                _technologyRepository = technologyRepository;
                _mapper = mapper;
                _businessRules = businessRules;
            }

            public async Task<TechnologyGetByIdDto> Handle(GetByIdTechnologyQuery request, CancellationToken cancellationToken)
            {
                Technology technology = await _technologyRepository.GetAsync(t => t.Id == request.Id);

                await _businessRules.TechnologyShouldExistWhenRequested(request.Id);

                TechnologyGetByIdDto technologyGetByIdDto = _mapper.Map<TechnologyGetByIdDto>(technology);

                return technologyGetByIdDto;

            }
        }
    }
}
