using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Interfaces
{
    /// <summary>
    /// Service for token management
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generate and set tokens
        /// </summary>
        /// <param name="user">Token owner</param>
        /// <param name="httpContext">Http context</param>
        /// <param name="usersRepository">DB repository</param>
        /// <returns></returns>
        Task GenerateTokens(User user, HttpContext httpContext, IUsersRepository usersRepository);

        /// <summary>
        /// Generate access token
        /// </summary>
        /// <param name="user">Athenticated user</param>
        /// <param name="lifeTimeInDays">Access token life time in days</param>
        /// <returns>Access token</returns>
        string GenerateAccessToken(User user, double lifeTimeInDays);

        /// <summary>
        /// Generate refresh token itself
        /// </summary>
        /// <returns>Refresh token</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Generate refresh token object, which contains tokent itself and lifetime
        /// </summary>
        /// <param name="lifeTimeInDays">Refresh token life time in days</param>
        /// <returns>Refresh token</returns>
        RefreshToken GenerateRefreshToken(double lifeTimeInDays);

        /// <summary>
        /// Validate access token
        /// </summary>
        /// <param name="accessToken">Access token to validate</param>
        /// <param name="id">Id from token</param>
        /// <returns>True if valid; else - false</returns>
        bool ValidateAccessToken(string accessToken, out string id);

        /// <summary>
        /// Validate refresh token
        /// </summary>
        /// <param name="user">Token owner</param>
        /// <param name="refreshToken">Refresh token to validate</param>
        /// <returns>True if valid; else - false</returns>
        bool ValidateRefreshToken(User user, string refreshToken);
    }
}
