using AutoMapper;
using UserService.BLL.Exceptions;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class UsersService(
        IUserRepository userRepository, 
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IUsersService
    {
        public async Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException("User with such id does not exist");

            return mapper.Map<User>(user);
        }

        public async Task<List<User>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetActivatedUsersAsync(page, pageSize, cancellationToken);

            return mapper.Map<List<User>>(user);
        }

        public async Task UpdateAsyc(Guid userId, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            var userEntity = await userRepository.GetByIdAsync(userId, cancellationToken);
            
            if(userEntity is null)
            {
                throw new NotFoundException("There is no user with this id");
            }

            userEntity.FirstName = firstName;
            userEntity.LastName = lastName;

            userRepository.Update(userEntity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRoleToUser(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken)
                 ?? throw new NotFoundException("User with such id does not exist");

            _ = await roleRepository.GetByIdAsync(roleId, cancellationToken)
                 ?? throw new NotFoundException("Role with such id does not exist");

            user.RoleId = roleId;

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
