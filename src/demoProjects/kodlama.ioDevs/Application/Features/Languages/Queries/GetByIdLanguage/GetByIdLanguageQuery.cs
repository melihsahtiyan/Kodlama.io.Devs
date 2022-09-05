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

namespace Application.Features.Languages.Queries.GetByIdLanguage
{
    public class GetByIdLanguageQuery : IRequest<LanguageGetByIdDto>
    {
        public int Id { get; set; }

        public class GetByIdLanguageQueryHandler : IRequestHandler<GetByIdLanguageQuery, LanguageGetByIdDto>
        {
            private readonly IMapper _mapper;
            private readonly ILanguageRepository _languageRepository;

            public GetByIdLanguageQueryHandler(IMapper mapper, ILanguageRepository languageRepository)
            {
                _mapper = mapper;
                _languageRepository = languageRepository;
            }

            public async Task<LanguageGetByIdDto> Handle(GetByIdLanguageQuery request, CancellationToken cancellationToken)
            {
                Language language = await _languageRepository.GetAsync(l => l.Id == request.Id);

                LanguageGetByIdDto languageGetByIdDto= _mapper.Map<LanguageGetByIdDto>(language);

                return languageGetByIdDto;
            }
        }
    }
}
