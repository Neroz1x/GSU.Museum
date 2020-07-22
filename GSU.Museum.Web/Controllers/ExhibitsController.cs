﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.Web.Controllers
{
    public class ExhibitsController : Controller
    {
        private readonly IExhibitsRepository _exhibitsRepository;
        private readonly IFormFileToByteConverterService _formFileToByteConverterService;

        public ExhibitsController(IExhibitsRepository exhibitsRepository, IFormFileToByteConverterService formFileToByteConverterService)
        {
            _exhibitsRepository = exhibitsRepository;
            _formFileToByteConverterService = formFileToByteConverterService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string hallId, string standId, string id)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            return View(await _exhibitsRepository.GetAsync(hallId, standId, id));
        }

        [HttpGet]
        public IActionResult Create(string hallId, string standId)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files)
        {
            if (files.Count() != 0)
            {
                await _formFileToByteConverterService.ConvertAsync(files, exhibit);
            }
            await _exhibitsRepository.CreateAsync(hallId, standId, exhibit);
            return RedirectToAction("MuseumManagement", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string hallId, string standId, string id)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            return View(await _exhibitsRepository.GetAsync(hallId, standId, id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> ids)
        {
            var initialExhibit = await _exhibitsRepository.GetAsync(hallId, standId, exhibit.Id);

            if (initialExhibit.Photos != null)
            {
                exhibit.Photos = initialExhibit.Photos;
                foreach(var photo in exhibit.Photos)
                {
                    if(!ids.Contains(photo?.Id))
                    {
                        photo.Photo = null;
                    }
                }
            }

            if (files.Count() != 0)
            {
                await _formFileToByteConverterService.ConvertAsync(files, exhibit);
            }

            await _exhibitsRepository.UpdateAsync(hallId, standId, exhibit.Id, exhibit);
            return RedirectToAction("MuseumManagement", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string hallId, string standId, string id)
        {
            await _exhibitsRepository.RemoveAsync(hallId, standId, id);
            return RedirectToAction("MuseumManagement", "Home");
        }
    }
}
