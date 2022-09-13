using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Core.CrossCuttingConcers.Exceptions;
using Core.Persistence.Paging;
using Domain.Entities;

namespace Application.Features.Technologies.Rules
{
    public class TechnologyBusinessRules
    {
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ILanguageRepository _languageRepository;

        public TechnologyBusinessRules(ITechnologyRepository technologyRepository, ILanguageRepository languageRepository)
        {
            _technologyRepository = technologyRepository;
            _languageRepository = languageRepository;
        }

        public async Task LanguageShouldExistWhenTechnologyInserted(int languageId)
        {
            Language? language = await _languageRepository.GetAsync(l => l.Id == languageId);

            if (language == null)
                throw new BusinessException("Language must exist when Technology inserted");
        }

        public async Task TechnologyShouldExistWhenRequested(int? id)
        {
            IPaginate<Technology> technology = await _technologyRepository.GetListAsync(t => t.Id == id);

            if (!technology.Items.Any())
                throw new BusinessException("Requested Technology does not exist");
        }

        public async Task TechnologyNameCanNotBeDuplicatedWhenRequested(string name)
        {
            IPaginate<Technology> technology = await _technologyRepository.GetListAsync(t => t.Name == name);
            
            if (technology.Items.Any())
                throw new BusinessException("Requested Technology can not be duplicated");
        }

    }
}
