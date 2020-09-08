using GSU.Museum.Web.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Middleware
{
    /// <summary>
    /// Middleware to process JWT
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUsersRepository usersRepository, ITokenService tokenService)
        {
            // Get tokens
            context.Request.Headers.TryGetValue("refresh_token", out StringValues refreshToken);
            context.Request.Headers.TryGetValue("access_token", out StringValues accessToken);

            if (refreshToken.Count != 0 && accessToken.Count != 0)
            {
                await AttachUserToContext(context, usersRepository, tokenService, accessToken, refreshToken);
            }

            await _next(context);
        }

        // Validate tokens and attach user to http context
        private async Task AttachUserToContext(HttpContext context, IUsersRepository usersRepository, ITokenService tokenService, string accessToken, string refreshToken)
        {
            try
            {
                // Validate Access Token 
                if(!tokenService.ValidateAccessToken(accessToken, out string userId))
                {
                    var user = await usersRepository.GetByIdAsync(userId);
                    context.Items["IsAuthorised"] = false;
                    context.Items["User"] = user;

                    // Validate Refresh Token
                    if (tokenService.ValidateRefreshToken(user, refreshToken))
                    {
                        await tokenService.GenerateTokens(user, context, usersRepository);
                        context.Items["IsAuthorised"] = true;
                    }
                }
                else
                {
                    if (userId != null)
                    {
                        context.Items["User"] = await usersRepository.GetByIdAsync(userId);
                        context.Items["IsAuthorised"] = true;
                    }
                }
            }
            catch
            {
                // Do nothing. It will be processed in OnAuthorization
            }
        }
    }
}
