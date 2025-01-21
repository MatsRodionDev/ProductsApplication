using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using Shared.Consts;
using Shared.Contracts;
using Shared.Enums;
using System.Threading;
using UserService.BLL.Common.Dtos;
using UserService.BLL.Common.Providers;
using UserService.BLL.Common.Responses;
using UserService.BLL.Exceptions;
using UserService.BLL.Interfaces.Hashers;
using UserService.BLL.Interfaces.Jobs;
using UserService.BLL.Interfaces.MessageBroker;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class AuthService(
        ICacheService cacheService,
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        IBackgroundJobClient backgroundJobClient,
        IEventBus eventBus,
        IMapper mapper,
        ILogger<AuthService> logger) : IAuthService
    {

        public async Task ActivateAsync(Guid userId, int activatePass, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Activating user with ID: {UserId}", userId);

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (userEntity is null)
            {
                logger.LogWarning("User with ID: {UserId} not found", userId);
                throw new BadRequestException("User with such id does not exist");
            }

            if (userEntity.IsActivated)
            {
                logger.LogWarning("User with ID: {UserId} is already activated", userId);
                throw new BadRequestException("An account with this ID has already been activated.");
            }

            await ValidateActivateCodeAsync(userId, activatePass, cancellationToken);

            userEntity.IsActivated = true;
            unitOfWork.UserRepository.Update(userEntity);

            await cacheService.RemoveAsync(
                CacheKeysProvider.GetActivateKey(userId),
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await eventBus.PublishAsync(new UserActivatedEvent(userEntity.Id));

            logger.LogInformation("User with ID: {UserId} successfully activated", userId);
        }

        public async Task<TokenResponse> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting login for email: {Email}", email);

            var userEntity = await unitOfWork.UserRepository.GetByEmailAsync(email, cancellationToken);

            await ValidateUserAsync(userEntity, password, cancellationToken);

            var user = mapper.Map<User>(userEntity);
            var role = user.Role!.Name;

            var accessToken = jwtService.GenerateAccesToken(user, role);
            var refreshToken = jwtService.GenerateRefreshToken(user.Id);

            await cacheService.SetAsync(
                CacheKeysProvider.GetRefreshKey(user.Id),
                refreshToken,
                TimeSpan.FromDays(30),
                cancellationToken);

            logger.LogInformation("User with ID: {UserId} successfully logged in", user.Id);

            return new TokenResponse(accessToken, refreshToken);
        }

        public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Logging out user with ID: {UserId}", userId);

            await cacheService.RemoveAsync(
                CacheKeysProvider.GetRefreshKey(userId),
                cancellationToken);

            logger.LogInformation("User with ID: {UserId} successfully logged out", userId);
        }

        public async Task<TokenResponse> RefreshAsync(string? refreshToken, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Refreshing token");

            var userId = GetUserIdFromToken(refreshToken);

            await ValidateRefreshTokenAsync(userId, refreshToken!, cancellationToken);

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (userEntity is null)
            {
                logger.LogWarning("User with ID: {UserId} not found during token refresh", userId);
                throw new UnauthorizedException();
            }

            var user = mapper.Map<User>(userEntity);
            var role = user.Role!.Name;
            var newAccessToken = jwtService.GenerateAccesToken(user, role);

            logger.LogInformation("Token refreshed for user ID: {UserId}", userId);

            return new TokenResponse(newAccessToken, refreshToken!);
        }

        public async Task<Guid> GenerateNewActivateCodeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Generating new activation code for user with ID: {UserId}", id);

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

            if (userEntity is null)
            {
                logger.LogWarning("User with ID: {UserId} not found for activation code generation", id);
                throw new NotFoundException("User with the specified ID does not exist.");
            }

            if (userEntity.IsActivated)
            {
                logger.LogWarning("User with ID: {UserId} is already activated", id);
                throw new BadRequestException("The account is already activated.");
            }

            await SendActivateCodeAsync(userEntity, cancellationToken);

            logger.LogInformation("Activation code sent for user with ID: {UserId}", id);

            return userEntity.Id;
        }

        public async Task<Guid> RegisterAsync(User newUser, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Registering new user with email: {Email}", newUser.Email);

            var user = await unitOfWork.UserRepository
                .GetByEmailAsync(newUser.Email, cancellationToken);

            if (user is not null)
            {
                logger.LogWarning("User with email: {Email} already exists", newUser.Email);
                throw new BadRequestException("User with such email already exists");
            }

            var role = await unitOfWork.RoleRepository.GetByNameAsync(RoleEnum.User.ToString(), cancellationToken);

            if (role is null)
            {   
                logger.LogError("No roles found on the server");
                throw new Exception("There are no roles in the server");
            }

            newUser.Id = Guid.NewGuid();
            newUser.RoleId = role.Id;
            newUser.PasswordHash = passwordHasher.Generate(newUser.PasswordHash);

            var userEntity = mapper.Map<UserEntity>(newUser);

            await unitOfWork.UserRepository.CreateAsync(userEntity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            await SendActivateCodeAsync(userEntity, cancellationToken);

            logger.LogInformation("User with ID: {UserId} successfully registered", userEntity.Id);

            return userEntity.Id;
        }

        private Guid GetUserIdFromToken(string? refreshToken)
        {
            if (refreshToken is null)
            {
                logger.LogWarning("Refresh token is null");
                throw new UnauthorizedException();
            }

            var claims = jwtService.GetClaimsFromToken(refreshToken);

            if (claims is null)
            {
                logger.LogWarning("Claims not found in refresh token");
                throw new UnauthorizedException();
            }

            if (!claims.TryGetValue(CustomClaims.USER_ID_CLAIM_KEY, out var id) || id == null || !Guid.TryParse(id.ToString(), out var userId))
            {
                logger.LogWarning("Invalid user ID in token claims");
                throw new UnauthorizedException();
            }

            return userId;
        }

        private async Task SendActivateCodeAsync(UserEntity userEntity, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Sending activation code to user with ID: {UserId}, Email: {Email}", userEntity.Id, userEntity.Email);

            var activateCode = CodeProvider.GenerateSixDigitCode();

            await cacheService.SetAsync(
                CacheKeysProvider.GetActivateKey(userEntity.Id),
                activateCode,
                TimeSpan.FromMinutes(1),
                cancellationToken);

            backgroundJobClient.Enqueue<IUserJobsService>(
                job => job.SendActivateCode(userEntity.Email, activateCode));

            logger.LogInformation("Activation code sent to user with ID: {UserId}", userEntity.Id);
        }

        private async Task ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var token = await cacheService.GetAsync<string>(
                CacheKeysProvider.GetRefreshKey(userId),
                cancellationToken);

            if (token is null)
            {
                logger.LogWarning("Refresh token not found for user with ID: {UserId}", userId);
                throw new UnauthorizedException();
            }

            if (token != refreshToken)
            {
                logger.LogWarning("Invalid refresh token for user with ID: {UserId}", userId);
                throw new UnauthorizedException();
            }
        }

        private async Task ValidateActivateCodeAsync(Guid userId, int activatePass, CancellationToken cancellationToken = default)
        {
            var code = await cacheService.GetAsync<int>(
                CacheKeysProvider.GetActivateKey(userId),
                cancellationToken);

            if (code is 0)
            {
                logger.LogWarning("Activation code expired or not found for user with ID: {UserId}", userId);
                throw new BadRequestException("Activation code has expired or does not exist.");
            }

            if (code != activatePass)
            {
                logger.LogWarning("Invalid activation code for user with ID: {UserId}", userId);
                throw new BadRequestException("Incorrect code");
            }
        }

        private async Task ValidateUserAsync(UserEntity? userEntity, string password, CancellationToken cancellationToken = default)
        {
            if (userEntity is null)
            {
                logger.LogWarning("User not found during login validation");
                throw new BadRequestException("Incorrect email or password");
            }

            var token = await cacheService.GetAsync<string>(
                CacheKeysProvider.GetRefreshKey(userEntity.Id),
                cancellationToken);

            if (token is not null)
            {
                logger.LogWarning("User with ID: {UserId} is already logged in", userEntity.Id);
                throw new UnauthorizedException("This account is already logged in");
            }

            if (!userEntity.IsActivated)
            {
                logger.LogWarning("User with ID: {UserId} is not activated", userEntity.Id);
                throw new BadRequestException("This account has not been activated");
            }

            if (!passwordHasher.Verify(password, userEntity.PasswordHash))
            {
                logger.LogWarning("Incorrect password for user with ID: {UserId}", userEntity.Id);
                throw new BadRequestException("Incorrect email or password");
            }
        }
    }
}
