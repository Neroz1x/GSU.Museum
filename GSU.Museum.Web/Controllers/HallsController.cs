using System.Collections.Generic;
using System.Threading.Tasks;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.Web.Controllers
{
    public class HallsController : Controller
    {
        private readonly IHallsRepository _hallsRepository;
        private readonly IFormFileToByteConverterService _formFileToByteConverterService;

        public HallsController(IHallsRepository hallsRepository, IFormFileToByteConverterService formFileToByteConverterService)
        {
            _hallsRepository = hallsRepository;
            _formFileToByteConverterService = formFileToByteConverterService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            return View(await _hallsRepository.GetAsync(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HallViewModel hall, IFormFile file)
        {
            if (file != null)
            {
                await _formFileToByteConverterService.ConvertAsync(file, hall);
            }
            hall.Stands = new List<StandViewModel>();
            await _hallsRepository.CreateAsync(hall);
            return RedirectToAction("MuseumManagement", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return View(await _hallsRepository.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HallViewModel hall, byte[] photo, IFormFile file)
        {
            var initialHall = await _hallsRepository.GetAsync(hall.Id);
            hall.Stands = initialHall.Stands;

            if (initialHall.Photo != null)
            {
                hall.Photo = initialHall.Photo;
                if (photo == null)
                {
                    hall.Photo.Photo = null;
                }
            }

            if (file != null)
            {
                await _formFileToByteConverterService.ConvertAsync(file, hall);
            }
            
            await _hallsRepository.UpdateAsync(hall.Id, hall);
            return RedirectToAction("MuseumManagement", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _hallsRepository.RemoveAsync(id);
            return RedirectToAction("MuseumManagement", "Home");
        }
    }
}
