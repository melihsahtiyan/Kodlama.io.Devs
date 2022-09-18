using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : UserForRegisterDto, IRequest<TokenDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, TokenDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly ITokenHelper _tokenHelper;

            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper,
                UserBusinessRules businessRules, ITokenHelper tokenHelper, IUserOperationClaimRepository userOperationClaimRepository)
                => (_userRepository, _mapper, _userBusinessRules, _tokenHelper, _userOperationClaimRepository) =
                    (userRepository, mapper, businessRules, tokenHelper, userOperationClaimRepository);
            

            public async Task<TokenDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.EmailCanNotBeDuplicatedWhenInserted(request.Email);

                UserForRegisterDto registerDto = _mapper.Map<UserForRegisterDto>(request);
                //request.Password = "";

                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash,
                    out passwordSalt);
                //registerDto.Password = "";

                User mappedUser = _mapper.Map<User>(registerDto);
                mappedUser.PasswordHash = passwordHash;
                mappedUser.PasswordSalt = passwordSalt;
                mappedUser.Status = true;

                User createdUser = await _userRepository.AddAsync(mappedUser);
                UserOperationClaim userOperationClaim = new()
                    {OperationClaimId = 2, UserId = createdUser.Id};

                userOperationClaim = await _userOperationClaimRepository.AddAsync(userOperationClaim);


                AccessToken token = _tokenHelper.CreateToken(createdUser,
                    new List<OperationClaim>());

                TokenDto createTokenDto = _mapper.Map<TokenDto>(token);

                return createTokenDto;
            }
        }
    }
}
