using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Consts;
using UserService.API.Dtos.Requests;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;

namespace UserService.API.Controllers
{
    [Controller]
    [Route("api/auth")]
    public class AuthController(
        IAuthService authService,
        IMapper mapper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequest dto, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(dto);

            var id = await authService.RegisterAsync(user, cancellationToken);

            return Ok(id);
        }

        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateAccountRequest dto, CancellationToken cancellationToken)
        {
            await authService.ActivateAsync(dto.UserId, dto.ActivateCode, cancellationToken);

            return Ok();
        }

        [HttpPost("code")]
        public async Task<IActionResult> Code([FromBody] Guid uderId, CancellationToken cancellationToken)
        {
           var id =  await authService.GenerateNewActivateCodeAsync(uderId, cancellationToken);

            return Ok(id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest dto, CancellationToken cancellationToken)
        {
            var tokens = await authService.LoginAsync(dto.Email, dto.Password, cancellationToken);

            Response.Cookies.Append(CookiesConstants.ACCESS, tokens.AccessToken);
            Response.Cookies.Append(CookiesConstants.REFRESH, tokens.RefreshToken);

            return Created();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            var token = Request.Cookies[CookiesConstants.REFRESH];

            var tokens = await authService.RefreshAsync(token, cancellationToken);

            Response.Cookies.Append(CookiesConstants.ACCESS, tokens.AccessToken);
            Response.Cookies.Append(CookiesConstants.REFRESH, tokens.RefreshToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.USER)]
        [HttpPatch("logout")]
        public async Task<IActionResult> LogOut(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);

            await authService.LogoutAsync(userId, cancellationToken);

            Response.Cookies.Delete(CookiesConstants.ACCESS);
            Response.Cookies.Delete(CookiesConstants.REFRESH);

            return Ok();
        }
    }
}
