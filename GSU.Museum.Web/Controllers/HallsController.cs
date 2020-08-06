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
        public async Task<PartialViewResult> Index(string id)
        {
            return PartialView("~/Views/Shared/Halls/_Index.cshtml", await _hallsRepository.GetAsync(id));
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            return PartialView("~/Views/Shared/Halls/_Create.cshtml");
        }

        [HttpPost]
        public async Task<string> Create(HallViewModel hall, IFormFile file)
        {
            if (file != null)
            {
                await _formFileToByteConverterService.ConvertAsync(file, hall);
            }
            hall.Stands = new List<StandViewModel>();
            return await _hallsRepository.CreateAsync(hall);
        }

        [HttpGet]
        public async Task<PartialViewResult> Edit(string id)
        {
            return PartialView("~/Views/Shared/Halls/_Edit.cshtml", await _hallsRepository.GetAsync(id));
        }

        [HttpPost]
        public async Task Edit(HallViewModel hall, byte[] photo, IFormFile file)
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
        }

        [HttpGet]
        public void Delete(string id)
        {
            _hallsRepository.RemoveAsync(id);
        }
    }
}
