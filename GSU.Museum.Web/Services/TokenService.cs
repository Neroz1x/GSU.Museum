using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Services
{
    public class TokenService : ITokenService
    {
        private const string KeyWord = "1528e11a-7ff7-4e3e-b8f3-e5467613990b";
        
        public string GenerateAccessToken(User user, double lifeTimeInDays)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(KeyWord);

            // Set claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Login)
            };

            var claimsDictionary = new Dictionary<string, object>();

            foreach (var claim in claims)
            {
                claimsDictionary.Add(claim.Type, claim.Value);
            }

            // Generate token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Claims = claimsDictionary,
                Expires = DateTime.UtcNow.AddDays(lifeTimeInDays),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public RefreshToken GenerateRefreshToken(double lifeTimeInDays)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                Token = GenerateRefreshToken(),
                ExpirationTime = DateTime.UtcNow.AddDays(lifeTimeInDays)
            };
            return refreshToken;
        }

        public async Task GenerateTokens(User user, HttpContext httpContext, IUsersRepository usersRepository)
        {
            // Tokens life time in days 
            const double AccessTokenLifeTimeInDays = 1;
            const double RefeshTokenLifeTimeInDays = 7;

            // Generate tokens
            var newAccessToken = GenerateAccessToken(user, AccessTokenLifeTimeInDays);
            var newRefreshToken = GenerateRefreshToken(RefeshTokenLifeTimeInDays);

            user.RefreshToken = newRefreshToken;
            await usersRepository.UpdateAsync(user.Id, user);

            // Set cookies
            httpContext.Response.OnStarting(state =>
            {
                var context = (HttpContext)state;

                context.Response.Cookies.Append("GSU.Museum.Web.AccessToken", newAccessToken, new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(RefeshTokenLifeTimeInDays), 
                    Secure = true
                });
                context.Response.Cookies.Append("GSU.Museum.Web.RefreshToken", newRefreshToken.Token, new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(RefeshTokenLifeTimeInDays),
                    Secure = true
                });
                return Task.CompletedTask;
            }, httpContext);
        }

        public bool ValidateAccessToken(string token, out string id)
        {
            // Validate token
            var tokenValidationParamters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateActor = false,
                ValidateLifetime = false, 
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(KeyWord)
                    )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParamters, out SecurityToken securityToken);

            // Get user id from token
            id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || jwtSecurityToken?.ValidTo < DateTime.UtcNow)
            {
                return false;
            }
            
            return true;
        }

        public bool ValidateRefreshToken(User user, string refreshToken)
        {
            if (user == null || !user.RefreshToken.Token.Equals(refreshToken))
            {
                return false;
            }

            if (DateTime.UtcNow > user.RefreshToken.ExpirationTime)
            {
                return false;
            }
            return true;
        }
    }
}
