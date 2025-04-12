using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Consts;
using UserService.BLL.Interfaces.Services;

namespace UserService.API.Controllers
{
    [Authorize(Policy = Policies.ADMIN)]
    [Controller]
    [Route("api/roles")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var roles = await roleService.GetAllRolesAsync(cancellationToken);

            return Ok(roles);
        }
    }
}
