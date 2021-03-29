using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GSU.Museum.Web.Models;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Attributes;
using System.Net.Http;
using GSU.Museum.CommonClassLibrary.Constants;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace GSU.Museum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _homeService = homeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MuseumManagement()
        {
            return View(await _homeService.GetAllAsync());
        }
        
        [Authorize]
        public IActionResult CacheManagement()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCache(bool isRussianChecked, bool isEnglishChecked, bool isBelarussianChecked, bool isPhotosChecked)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", "U3VwZXJTZWNyZXRBcGlLZXkxMjM");

            var list = new List<string>
            {
                isRussianChecked == true ? LanguageConstants.LanguageRu : string.Empty,
                isEnglishChecked == true ? LanguageConstants.LanguageEn : string.Empty,
                isBelarussianChecked == true ? LanguageConstants.LanguageBy : string.Empty
            };

            list = list.Where(item => !string.IsNullOrEmpty(item)).ToList();

            var response = await httpClient.PostAsync($"https://gsumuseumapi.azurewebsites.net/api/Cache?savePhotos={isPhotosChecked}", new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
