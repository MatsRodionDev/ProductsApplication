using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Consts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.BLL.Common;
using UserService.BLL.Interfaces.Services;
using UserService.BLL.Models;

namespace UserService.BLL.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateAccesToken(User user, string role)
        {

            Claim[] claims =
            [
                new (CustomClaims.USER_ROLE_CLAIM_KEY, role),
                new (CustomClaims.USER_ID_CLAIM_KEY, user.Id.ToString())
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccesTokenExpiresMinutes));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public string GenerateRefreshToken(Guid userId)
        {
            Claim[] claims =
            [
                new (CustomClaims.USER_ID_CLAIM_KEY, userId.ToString()),
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiresDays));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public IDictionary<string, string>? GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                return null;
            }

            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

            return claims;
        }
    }
}
