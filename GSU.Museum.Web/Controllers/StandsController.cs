using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.Web.Controllers
{
    public class StandsController : Controller
    {
        private readonly IStandsRepository _standsRepository; 
        private readonly IFormFileToByteConverterService _formFileToByteConverterService;

        public StandsController(IStandsRepository standsRepository, IFormFileToByteConverterService formFileToByteConverterService)
        {
            _standsRepository = standsRepository;
            _formFileToByteConverterService = formFileToByteConverterService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string hallId, string id)
        {
            ViewBag.HallId = hallId;
            return View(await _standsRepository.GetAsync(hallId, id));
        }

        [HttpGet]
        public IActionResult Create(string hallId)
        {
            ViewBag.HallId = hallId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string hallId, StandViewModel stand, IFormFile file)
        {
            if (file != null)
            {
                await _formFileToByteConverterService.ConvertAsync(file, stand);
            }
            stand.Exhibits = new List<ExhibitViewModel>();
            await _standsRepository.CreateAsync(hallId, stand);
            return RedirectToAction("MuseumManagement", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string hallId, string id)
        {
            ViewBag.HallId = hallId;
            return View(await _standsRepository.GetAsync(hallId, id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string hallId, StandViewModel stand, IFormFile file, IEnumerable<string> exhibits, byte[] photo)
        {
            var initialStand = await _standsRepository.GetAsync(hallId, stand.Id);
            stand.Exhibits = new List<ExhibitViewModel>();
            foreach(var id in exhibits)
            {
                stand.Exhibits.Add(initialStand.Exhibits.First(e => e.Id.Equals(id)));
            }

            if (initialStand.Photo != null)
            {
                stand.Photo = initialStand.Photo;
                if (photo == null)
                {
                    stand.Photo.Photo = null;
                }
            }

            if (file != null)
            {
                await _formFileToByteConverterService.ConvertAsync(file, stand);
            }
            
            await _standsRepository.UpdateAsync(hallId, stand.Id, stand);
            return RedirectToAction("MuseumManagement", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string hallId, string id)
        {
            await _standsRepository.RemoveAsync(hallId, id);
            return RedirectToAction("MuseumManagement", "Home");
        }
    }
}
