﻿using AutoMapper;
using Shared.Consts;
using Shared.Enums;
using UserService.BLL.Common.Dtos;
using UserService.BLL.Common.Providers;
using UserService.BLL.Common.Responses;
using UserService.BLL.Exceptions;
using UserService.BLL.Interfaces.Hashers;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class AuthService(
        ICacheService cacheService,
        IEmailService emailService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper) : IAuthService
    {

        public async Task ActivateAsync(Guid userId, int activatePass, CancellationToken cancellationToken = default)
        {
            var userEntity = await userRepository.GetByIdAsync(userId, cancellationToken);

            if (userEntity is null)
            {
                throw new BadRequestException("User with such id does not exist");
            }

            if (userEntity.IsActivated)
            {
                throw new BadRequestException("An account with this ID has already been activated.");
            }

            await ValidateActivatePassAsync(userId, activatePass, cancellationToken);

            userEntity.IsActivated = true;
            userRepository.Update(userEntity);

            await cacheService.RemoveAsync(
                CacheKeysProvider.GetActivateKey(userId),
                cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<TokenResponse> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var userEntity = await userRepository.GetByEmailAsync(email, cancellationToken);

            if (userEntity is null)
            {
                throw new BadRequestException("Incorrect email or password");
            }

            var token = await cacheService.GetAsync<string>(
                CacheKeysProvider.GetRefreshKey(userEntity.Id),
                cancellationToken);

            if(token is not null)
            {
                throw new UnauthorizedException("This account is already logged in");
            }

            if (!userEntity.IsActivated)
            {
                throw new BadRequestException("This account has not been activated");
            }

            var isPasswordValid = passwordHasher.Verify(password, userEntity.PasswordHash);

            if (!isPasswordValid)
            {
                throw new BadRequestException("Incorrect email or password");
            }

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

            var userEntity = await userRepository.GetByIdAsync(userId, cancellationToken);

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
            var userEntity = await userRepository.GetByIdAsync(id, cancellationToken); 

            if (userEntity is null)
            {
                throw new NotFoundException("User with the specified ID does not exist.");
            }

            if (userEntity.IsActivated)
            {
                throw new BadRequestException("The account is already activated.");
            }

            await GenerateAcivateCodeAsync(userEntity, cancellationToken);

            return userEntity.Id;
        }

        public async Task<Guid> RegisterAsync(User newUser, CancellationToken cancellationToken = default)
        {
            var user = await userRepository
                .GetByEmailAsync(newUser.Email, cancellationToken);

            if(user is not null)
            {
                throw new BadRequestException("User with such email already exist");
            }

            var role = await roleRepository.GetByNameAsync(RoleEnum.User.ToString(), cancellationToken);

            if (role is null)
            {
                throw new Exception("There are no roles in the server");
            }

            newUser.Id = Guid.NewGuid();
            newUser.RoleId = role.Id;

            newUser.PasswordHash = passwordHasher.Generate(newUser.PasswordHash);

            var userEntity = mapper.Map<UserEntity>(newUser);

            await userRepository.CreateAsync(userEntity, cancellationToken);

            await unitOfWork.SaveChangesAsync();

            //I'll move it to an event later
            await GenerateAcivateCodeAsync(userEntity, cancellationToken);

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

        private async Task GenerateAcivateCodeAsync(UserEntity userEntity, CancellationToken cancellationToken = default)
        {
            var activateCode = CodeProvider.GenerateSixDigitCode();

            await cacheService.SetAsync(
                CacheKeysProvider.GetActivateKey(userEntity.Id),
                activateCode,
                TimeSpan.FromMinutes(1),
                cancellationToken);

            await emailService.SendEmail(new EmailDto
            {
                To = userEntity.Email,
                Subject = "Activation code",
                Body = activateCode.ToString()
            },
            cancellationToken);
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

        private async Task ValidateActivatePassAsync(Guid userId, int activatePass, CancellationToken cancellationToken = default)
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
    }
}