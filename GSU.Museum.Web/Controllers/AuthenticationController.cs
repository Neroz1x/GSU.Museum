using System.Threading.Tasks;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GSU.Museum.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUsersRepository _userRepository;

        public AuthenticationController(ITokenService tokenService, IUsersRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(User user)
        {
            // Validate user credential information
            var dbUser = await _userRepository.GetAsync(user.Login);
            if (dbUser == null || !user.Password.Equals(dbUser.Password))
            {
                ModelState.AddModelError("", "Неверные учетные данные");
                return PartialView("Index", user);
            }

            await _tokenService.GenerateTokens(dbUser, HttpContext, _userRepository);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Revoke(string login)
        {
            var dbUser = await _userRepository.GetAsync(login);

            // Delete user's refresh token
            dbUser.RefreshToken = null;
            await _userRepository.UpdateAsync(dbUser.Id, dbUser);

            // Remove client's tokens 
            HttpContext.Response.Cookies.Delete("GSU.Museum.Web.AccessToken");
            HttpContext.Response.Cookies.Delete("GSU.Museum.Web.RefreshToken");

            return NoContent();
        }
    }
}
