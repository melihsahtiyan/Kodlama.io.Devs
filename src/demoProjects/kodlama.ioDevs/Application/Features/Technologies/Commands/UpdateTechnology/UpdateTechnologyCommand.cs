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

namespace Application.Features.Technologies.Commands.UpdateTechnology
{
    public class UpdateTechnologyCommand : IRequest<UpdatedTechnologyDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LanguageId { get; set; }

        public class UpdateTechnologyCommandHandler : IRequestHandler<UpdateTechnologyCommand, UpdatedTechnologyDto>
        {
            private readonly ITechnologyRepository _technologyRepository;
            private readonly IMapper _mapper;
            private readonly TechnologyBusinessRules _businessRules;

            public UpdateTechnologyCommandHandler(ITechnologyRepository technologyRepository, IMapper mapper, TechnologyBusinessRules technologyBusinessRules)
            {
                _technologyRepository = technologyRepository;
                _mapper = mapper;
                _businessRules = technologyBusinessRules;
            }

            public async Task<UpdatedTechnologyDto> Handle(UpdateTechnologyCommand request, CancellationToken cancellationToken)
            {
                Technology? technologyToUpdate = await _technologyRepository.GetAsync(t => t.Id == request.Id);
                
                await _businessRules.TechnologyNameCanNotBeDuplicatedWhenRequested(request.Name);
                await _businessRules.LanguageShouldExistWhenTechnologyInserted(technologyToUpdate.LanguageId);

                Technology mappedTechnology = _mapper.Map<Technology>(technologyToUpdate);
                Technology updatedTechnology = await _technologyRepository.UpdateAsync(mappedTechnology);

                UpdatedTechnologyDto updatedTechnologyDto = _mapper.Map<UpdatedTechnologyDto>(updatedTechnology);

                return updatedTechnologyDto;
            }
        }
    }
}
