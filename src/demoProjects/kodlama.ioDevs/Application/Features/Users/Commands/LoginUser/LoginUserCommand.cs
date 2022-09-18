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

namespace Application.Features.Users.Commands.LoginUser
{
    public class LoginUserCommand : UserForLoginDto, IRequest<TokenDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly ITokenHelper _tokenHelper;

            public LoginUserCommandHandler (IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules, ITokenHelper tokenHelper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _tokenHelper = tokenHelper;
            }

            public async Task<TokenDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.EmailMustExistWhenLoggingIn(request.Email);

                UserForLoginDto loginDto = _mapper.Map<UserForLoginDto>(request);

                User requestedUser = await _userRepository.GetAsync(u => u.Email == loginDto.Email);

                await _userBusinessRules.CredentialsShouldMatch(request.Password, requestedUser.PasswordHash,
                    requestedUser.PasswordSalt);

                AccessToken token = _tokenHelper.CreateToken(requestedUser, new List<OperationClaim>());

                TokenDto tokenDto = _mapper.Map<TokenDto>(token);
                    
                return tokenDto;
            }
        }
    }
}
