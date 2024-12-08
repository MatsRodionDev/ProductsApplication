using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Consts;
using UserService.BLL.Interfaces.Services;

namespace UserService.API.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        [Authorize(Policy = Policies.ADMIN)]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var roles = await roleService.GetAllRolesAsync(cancellationToken);

            return Ok(roles);
        }
    }
}
