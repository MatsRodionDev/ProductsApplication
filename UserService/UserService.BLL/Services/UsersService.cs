using AutoMapper;
using Microsoft.Extensions.Logging;
using UserService.BLL.Exceptions;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class UsersService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UsersService> logger) : IUsersService
    {
        public async Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Getting user by ID: {UserId}", userId);

            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with ID: {UserId} not found", userId);
                throw new NotFoundException("User with such id does not exist");
            }

            logger.LogInformation("User with ID: {UserId} successfully retrieved", userId);

            return mapper.Map<User>(user);
        }

        public async Task<List<User>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Getting all users. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var users = await unitOfWork.UserRepository.GetActivatedUsersAsync(page, pageSize, cancellationToken);

            logger.LogInformation("Retrieved {UserCount} users for Page: {Page}, PageSize: {PageSize}", users.Count, page, pageSize);

            return mapper.Map<List<User>>(users);
        }

        public async Task UpdateAsyc(Guid userId, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Updating user with ID: {UserId}. New FirstName: {FirstName}, New LastName: {LastName}", userId, firstName, lastName);

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (userEntity is null)
            {
                logger.LogWarning("User with ID: {UserId} not found for update", userId);
                throw new NotFoundException("There is no user with this id");
            }

            userEntity.FirstName = firstName;
            userEntity.LastName = lastName;

            unitOfWork.UserRepository.Update(userEntity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User with ID: {UserId} successfully updated", userId);
        }

        public async Task UpdateRoleToUser(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Updating role for user with ID: {UserId} to Role ID: {RoleId}", userId, roleId);

            var user = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with ID: {UserId} not found for role update", userId);
                throw new NotFoundException("User with such id does not exist");
            }

            var role = await unitOfWork.RoleRepository.GetByIdAsync(roleId, cancellationToken);

            if (role is null)
            {
                logger.LogWarning("Role with ID: {RoleId} not found", roleId);
                throw new NotFoundException("Role with such id does not exist");
            }

            user.RoleId = roleId;

            unitOfWork.UserRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User with ID: {UserId} successfully updated to Role ID: {RoleId}", userId, roleId);
        }
    }
}
