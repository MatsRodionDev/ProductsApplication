using AutoMapper;
using Microsoft.Extensions.Logging;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class RoleService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<RoleService> logger) : IRoleService
    {
        public async Task<List<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Starting to fetch all roles from the repository");

            var roleEntities = await unitOfWork.RoleRepository.GetAllAsync(cancellationToken);

            logger.LogInformation("Successfully fetched roles from the repository. Count: {Count}", roleEntities.Count);

            return mapper.Map<List<Role>>(roleEntities);
        }
    }
}
