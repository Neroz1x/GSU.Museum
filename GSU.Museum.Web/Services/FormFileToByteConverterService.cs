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
    public class FormFileToByteConverterService : IFormFileToByteConverterService
    {
        public async Task ConvertAsync(IFormFile file, StandViewModel stand)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                if(stand.Photo == null)
                {
                    stand.Photo = new PhotoInfo();
                }
                stand.Photo.Photo = memoryStream.ToArray();
            }
        }

        public async Task ConvertAsync(IFormFile file, HallViewModel hall)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                if (hall.Photo == null)
                {
                    hall.Photo = new PhotoInfo();
                }
                hall.Photo.Photo = memoryStream.ToArray();
            }
        }

        public async Task ConvertAsync(IEnumerable<IFormFile> files, ExhibitViewModel exhibit)
        {
            if(exhibit.Photos == null)
            {
                exhibit.Photos = new List<PhotoInfo>();
            }
            
            foreach (var photo in files)
            {
                if (photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        exhibit.Photos.Add(new PhotoInfo() { Photo = memoryStream.ToArray() });
                    }
                }
            }
        }
    }
}
