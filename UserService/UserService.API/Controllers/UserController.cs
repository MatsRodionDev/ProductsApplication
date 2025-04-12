using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Consts;
using UserService.API.Dtos.Requests;
using UserService.API.Dtos.Responses;
using UserService.API.Extensions;
using UserService.BLL.Interfaces.Services;

namespace UserService.API.Controllers
{
    [Controller]
    [Route("api/users")]
    public class UserController(
        IUsersService userService,
        IMapper mapper) : ControllerBase
    {
        [Authorize(Policy = Policies.ADMIN)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await userService.GetByIdAsync(userId, cancellationToken);

            var userResponse = mapper.Map<UserResponse>(user);

            return Ok(userResponse);
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationRequest dto, CancellationToken cancellationToken)
        {
            var users = await userService.GetAllUsersAsync(dto.Page, dto.PageSize, cancellationToken);

            var usersResponse = mapper.Map<List<UserResponse>>(users);

            return Ok(usersResponse);
        }

        [Authorize(Policy = Policies.ADMIN)]
        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> UpdateRoleToUser(Guid userId, [FromBody] UpdateRoleToUserRequest dto, CancellationToken cancellationToken)
        {
            await userService.UpdateRoleToUser(userId, dto.RoleId, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.USER)]
        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequest dto, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await userService.UpdateAsyc(userId, dto.FirstName, dto.LastName, cancellationToken);

            return NoContent();
        }

        [Authorize(Policy = Policies.USER)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileIdAsync(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            var user = await userService.GetByIdAsync(userId, cancellationToken);

            var userResponse = mapper.Map<UserResponse>(user);

            return Ok(userResponse);
        }
    }
}
