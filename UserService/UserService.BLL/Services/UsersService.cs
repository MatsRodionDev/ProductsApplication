using AutoMapper;
using UserService.BLL.Exceptions;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class UsersService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IUsersService
    {
        public async Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException("User with such id does not exist");

            return mapper.Map<User>(user);
        }

        public async Task<List<User>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetActivatedUsersAsync(page, pageSize, cancellationToken);

            return mapper.Map<List<User>>(user);
        }

        public async Task UpdateAsyc(Guid userId, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);
            
            if(userEntity is null)
            {
                throw new NotFoundException("There is no user with this id");
            }

            userEntity.FirstName = firstName;
            userEntity.LastName = lastName;

            unitOfWork.UserRepository.Update(userEntity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRoleToUser(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken)
                 ?? throw new NotFoundException("User with such id does not exist");

            _ = await unitOfWork.RoleRepository.GetByIdAsync(roleId, cancellationToken)
                 ?? throw new NotFoundException("Role with such id does not exist");

            user.RoleId = roleId;

            unitOfWork.UserRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
