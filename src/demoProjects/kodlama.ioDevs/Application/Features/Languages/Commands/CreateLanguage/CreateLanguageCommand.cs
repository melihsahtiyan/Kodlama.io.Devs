using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Languages.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Languages.Commands.CreateLanguage
{
    public class CreateLanguageCommand : IRequest<CreatedLanguageDto>
    {
        public string Name { get; set; }

        public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand,CreatedLanguageDto>
        {
            private readonly ILanguageRepository _languageRepository;
            private readonly IMapper _mapper;

            public CreateLanguageCommandHandler(ILanguageRepository languageRepository, IMapper mapper)
            {
                _languageRepository = languageRepository;
                _mapper = mapper;
            }

            public async Task<CreatedLanguageDto> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
            {
                Language mappedLanguage = _mapper.Map<Language>(request);
                Language createdLanguage = await _languageRepository.AddAsync(mappedLanguage);
                CreatedLanguageDto createdLanguageDto = _mapper.Map<CreatedLanguageDto>(createdLanguage);

                return createdLanguageDto;
            }
        }
    }
}
