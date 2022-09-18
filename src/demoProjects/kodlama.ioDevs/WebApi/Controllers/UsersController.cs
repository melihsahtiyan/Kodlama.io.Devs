using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand createUserCommand)
        {
            TokenDto result = await Mediator.Send(createUserCommand);

            return Created("", result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            TokenDto result = await Mediator.Send(loginUserCommand);

            return Created("", result);
        }
    }
}
