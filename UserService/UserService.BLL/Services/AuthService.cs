using AutoMapper;
using Hangfire;
using Shared.Consts;
using Shared.Enums;
using System.Threading;
using UserService.BLL.Common.Dtos;
using UserService.BLL.Common.Providers;
using UserService.BLL.Common.Responses;
using UserService.BLL.Exceptions;
using UserService.BLL.Interfaces.Hashers;
using UserService.BLL.Interfaces.Jobs;
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
        IMapper mapper) : IAuthService
    {

        public async Task ActivateAsync(Guid userId, int activatePass, CancellationToken cancellationToken = default)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (userEntity is null)
            {
                throw new BadRequestException("User with such id does not exist");
            }

            if (userEntity.IsActivated)
            {
                throw new BadRequestException("An account with this ID has already been activated.");
            }

            await ValidateActivateCodeAsync(userId, activatePass, cancellationToken);

            userEntity.IsActivated = true;
            unitOfWork.UserRepository.Update(userEntity);

            await cacheService.RemoveAsync(
                CacheKeysProvider.GetActivateKey(userId),
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<TokenResponse> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
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

            return new TokenResponse(accessToken, refreshToken);
        }

        public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await cacheService.RemoveAsync(
                CacheKeysProvider.GetRefreshKey(userId),
                cancellationToken);
        }

        public async Task<TokenResponse> RefreshAsync(string? refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = GetUserIdFromToken(refreshToken);

            await ValidateRefreshTokenAsync(userId, refreshToken!, cancellationToken);

            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

            if (userEntity is null)
            {
                throw new UnauthorizedException();
            }

            var user = mapper.Map<User>(userEntity);
            var role = user.Role!.Name;
            var newAccessToken = jwtService.GenerateAccesToken(user, role);

            return new TokenResponse(newAccessToken, refreshToken!);
        }

        public async Task<Guid> GenerateNewActivateCodeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var userEntity = await unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken); 

            if (userEntity is null)
            {
                throw new NotFoundException("User with the specified ID does not exist.");
            }

            if (userEntity.IsActivated)
            {
                throw new BadRequestException("The account is already activated.");
            }

            await SendActivateCodeAsync(userEntity, cancellationToken);

            return userEntity.Id;
        }

        public async Task<Guid> RegisterAsync(User newUser, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.UserRepository
                .GetByEmailAsync(newUser.Email, cancellationToken);

            if(user is not null)
            {
                throw new BadRequestException("User with such email already exist");
            }

            var role = await unitOfWork.RoleRepository.GetByNameAsync(RoleEnum.User.ToString(), cancellationToken);

            if (role is null)
            {
                throw new Exception("There are no roles in the server");
            }

            newUser.Id = Guid.NewGuid();
            newUser.RoleId = role.Id;
            newUser.PasswordHash = passwordHasher.Generate(newUser.PasswordHash);

            var userEntity = mapper.Map<UserEntity>(newUser);

            await unitOfWork.UserRepository.CreateAsync(userEntity, cancellationToken);

            await unitOfWork.SaveChangesAsync();

            await SendActivateCodeAsync(userEntity, cancellationToken);

            return userEntity.Id;
        }

        private Guid GetUserIdFromToken(string? refreshToken)
        {
            if (refreshToken is null)
            {
                throw new UnauthorizedException();
            }

            var claims = jwtService.GetClaimsFromToken(refreshToken);

            if (claims is null)
            {
                throw new UnauthorizedException();
            }

            if (!claims.TryGetValue(CustomClaims.USER_ID_CLAIM_KEY, out var id) && id is not Guid)
            {
                throw new UnauthorizedException();
            }

            return Guid.Parse(id.ToString()!);
        }

        private async Task SendActivateCodeAsync(UserEntity userEntity, CancellationToken cancellationToken = default)
        {
            var activateCode = CodeProvider.GenerateSixDigitCode();

            await cacheService.SetAsync(
                CacheKeysProvider.GetActivateKey(userEntity.Id),
                activateCode,
                TimeSpan.FromMinutes(1),
                cancellationToken);

            backgroundJobClient.Enqueue<IUserJobs>(
                job => job.SendActivateCode(userEntity.Email, activateCode));
        }

        private async Task ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var token = await cacheService.GetAsync<string>(
                CacheKeysProvider.GetRefreshKey(userId),
                cancellationToken);

            if (token is null)  
            {
                throw new UnauthorizedException();
            }

            if (token != refreshToken)
            {
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
                throw new BadRequestException("Activation code has expired or does not exist.");
            }

            if (code != activatePass)
            {
                throw new BadRequestException("Incorrect code");
            }
        }

        private async Task ValidateUserAsync(UserEntity? userEntity, string password, CancellationToken cancellationToken = default)
        {
            if (userEntity is null)
            {
                throw new BadRequestException("Incorrect email or password");
            }

            var token = await cacheService.GetAsync<string>(
                CacheKeysProvider.GetRefreshKey(userEntity.Id),
                cancellationToken);

            if (token is not null)
            {
                throw new UnauthorizedException("This account is already logged in");
            }

            if (!userEntity.IsActivated)
            {
                throw new BadRequestException("This account has not been activated");
            }

            if (!passwordHasher.Verify(password, userEntity.PasswordHash))
            {
                throw new BadRequestException("Incorrect email or password");
            }
        }
    }
}
