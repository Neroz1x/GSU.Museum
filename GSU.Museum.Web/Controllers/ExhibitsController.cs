using System.Collections.Generic;
using System.Threading.Tasks;
using GSU.Museum.CommonClassLibrary.Data.Enums;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.Web.Controllers
{
    public class ExhibitsController : Controller
    {
        private readonly IExhibitsRepository _exhibitsRepository;
        private readonly IExhibitsService _exhibitsService;

        public ExhibitsController(IExhibitsRepository exhibitsRepository, IExhibitsService exhibitsService)
        {
            _exhibitsRepository = exhibitsRepository;
            _exhibitsService = exhibitsService;
        }
        [HttpGet]
        public async Task<PartialViewResult> Index(string hallId, string standId, string id)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            return PartialView("~/Views/Shared/Exhibits/_Index.cshtml", await _exhibitsRepository.GetAsync(hallId, standId, id));
        }

        [HttpGet]
        public PartialViewResult Create(string hallId, string standId)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            return PartialView("~/Views/Shared/Exhibits/_Create.cshtml");
        }

        [HttpGet]
        public PartialViewResult LoadCreateArticle(string hallId, string standId)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            return PartialView("~/Views/Shared/Exhibits/_CreateArticle.cshtml");
        }

        [HttpGet]
        public PartialViewResult LoadCreateGallery(string hallId, string standId)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            return PartialView("~/Views/Shared/Exhibits/_CreateGallery.cshtml");
        }

        [HttpPost]
        public async Task<string> Create(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> photoDescriptionBe, IEnumerable<string> photoDescriptionEn, IEnumerable<string> photoDescriptionRu)
        {
            exhibit = await _exhibitsService.CreateAsync(exhibit, files, photoDescriptionBe, photoDescriptionEn, photoDescriptionRu);
            return await _exhibitsRepository.CreateAsync(hallId, standId, exhibit);
        }

        [HttpGet]
        public async Task<PartialViewResult> Edit(string hallId, string standId, string id)
        {
            ViewBag.HallId = hallId;
            ViewBag.StandId = standId;
            var exhibit = await _exhibitsRepository.GetAsync(hallId, standId, id);
            if(exhibit.ExhibitType == ExhibitType.Article)
            {
                return PartialView("~/Views/Shared/Exhibits/_EditArticle.cshtml", exhibit);
            }
            return PartialView("~/Views/Shared/Exhibits/_EditGallery.cshtml", exhibit);
        }

        [HttpPost]
        public async Task EditArticle(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> ids)
        {
            exhibit = await _exhibitsService.EditArticleAsync(hallId, standId, exhibit, files, ids);
            await _exhibitsRepository.UpdateAsync(hallId, standId, exhibit.Id, exhibit);
        }

        [HttpPost]
        public async Task EditGallery(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> ids, IEnumerable<string> photoDescriptionBe, IEnumerable<string> photoDescriptionEn, IEnumerable<string> photoDescriptionRu)
        {
            exhibit = await _exhibitsService.EditGalleryAsync(hallId, standId, exhibit, files, ids, photoDescriptionBe, photoDescriptionEn, photoDescriptionRu);
            await _exhibitsRepository.UpdateAsync(hallId, standId, exhibit.Id, exhibit);
        }

        [HttpGet]
        public void Delete(string hallId, string standId, string id)
        {
            _exhibitsRepository.RemoveAsync(hallId, standId, id);
        }
    }
}
