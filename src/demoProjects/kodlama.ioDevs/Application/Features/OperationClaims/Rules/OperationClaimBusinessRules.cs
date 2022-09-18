using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using Core.CrossCuttingConcers.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.OperationClaims.Rules
{
    public class OperationClaimBusinessRules
    {
        private readonly IOperationClaimRepository _operationClaimRepository;

        public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository)
        {
            _operationClaimRepository = operationClaimRepository;
        }

        public async Task OperationClaimNameCanNotBeDublicatedWhenInserted(string name)
        {
            IPaginate<OperationClaim> result = await _operationClaimRepository.GetListAsync(o => o.Name == name);
            if (result.Items.Any()) throw new BusinessException("Operation Claim already exists");
        }

        public async Task OperationClaimMustExistWhenRequested(int id)
        {
            IPaginate<OperationClaim> result = await _operationClaimRepository.GetListAsync(o => o.Id == id);
            if (result.Items.IsNullOrEmpty()) throw new BusinessException("Operation Claim does not exist");
        }
    }
}
