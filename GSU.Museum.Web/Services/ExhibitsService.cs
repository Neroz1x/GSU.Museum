using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Services
{
    public class ExhibitsService : IExhibitsService
    {
        private readonly IExhibitsRepository _exhibitsRepository;
        private readonly IFormFileToByteConverterService _formFileToByteConverterService;

        public ExhibitsService(IExhibitsRepository exhibitsRepository, IFormFileToByteConverterService formFileToByteConverterService)
        {
            _exhibitsRepository = exhibitsRepository;
            _formFileToByteConverterService = formFileToByteConverterService;
        }

        public async Task<ExhibitViewModel> CreateAsync(ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> photoDescriptionBe, IEnumerable<string> photoDescriptionEn, IEnumerable<string> photoDescriptionRu)
        {
            if (files.Count() != 0)
            {
                await _formFileToByteConverterService.ConvertAsync(files, exhibit);
            }
            if (photoDescriptionBe?.Count() != 0 && photoDescriptionBe != null)
            {
                for (int i = 0; i < photoDescriptionBe.Count(); i++)
                {
                    exhibit.Photos[i].DescriptionBe = photoDescriptionBe.ElementAt(i);
                    exhibit.Photos[i].DescriptionEn = photoDescriptionEn.ElementAt(i);
                    exhibit.Photos[i].DescriptionRu = photoDescriptionRu.ElementAt(i);
                }
            }

            return exhibit;
        }

        public async Task<ExhibitViewModel> EditArticleAsync(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> ids)
        {
            var initialExhibit = await _exhibitsRepository.GetAsync(hallId, standId, exhibit.Id);

            if (initialExhibit.Photos != null)
            {
                exhibit.Photos = initialExhibit.Photos;
                foreach (var photo in exhibit.Photos)
                {
                    if (!ids.Contains(photo?.Id))
                    {
                        photo.Photo = null;
                    }
                }
            }

            if (files.Count() != 0)
            {
                await _formFileToByteConverterService.ConvertAsync(files, exhibit);
            }

            return exhibit;
        }

        public async Task<ExhibitViewModel> EditGalleryAsync(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> ids, IEnumerable<string> photoDescriptionBe, IEnumerable<string> photoDescriptionEn, IEnumerable<string> photoDescriptionRu)
        {
            var initialExhibit = await _exhibitsRepository.GetAsync(hallId, standId, exhibit.Id);

            exhibit.Photos = new List<PhotoInfo>();
            int fileIndex = 0;
            for (int i = 0; i < ids.Count(); i++)
            {
                exhibit.Photos.Add(new PhotoInfo()
                {
                    DescriptionBe = photoDescriptionBe.ElementAt(i),
                    DescriptionEn = photoDescriptionEn.ElementAt(i),
                    DescriptionRu = photoDescriptionRu.ElementAt(i),
                });
                if (ids.ElementAt(i) == "0")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await files.ElementAt(fileIndex++).CopyToAsync(memoryStream);
                        exhibit.Photos[i].Photo = memoryStream.ToArray();
                    }
                }
                else
                {
                    exhibit.Photos[i].Id = ids.ElementAt(i);
                    exhibit.Photos[i].Photo = initialExhibit.Photos.FirstOrDefault(p => p.Id.Equals(ids.ElementAt(i))).Photo;
                }
            }

            for (int i = 0; i < initialExhibit.Photos.Count(); i++)
            {
                if (!ids.Contains(initialExhibit.Photos[i].Id))
                {
                    exhibit.Photos.Add(new PhotoInfo()
                    {
                        Id = initialExhibit.Photos[i].Id
                    });
                }
            }

            return exhibit;
        }
    }
}
