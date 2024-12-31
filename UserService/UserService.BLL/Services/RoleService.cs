using AutoMapper;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class RoleService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRoleService
    {
        public async Task<List<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            var roleEntities = await unitOfWork.RoleRepository.GetAllAsync(cancellationToken);

            return mapper.Map<List<Role>>(roleEntities);
        }
    }
}
