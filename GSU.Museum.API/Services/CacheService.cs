using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Constants;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace GSU.Museum.API.Services
{
    public class CacheService : ICacheService
    {
        private readonly IHallsService _hallsService;
        private readonly IStandsService _standsService;
        private readonly IExhibitsService _exhibitsService;
        private bool _isPhotosSaved;

        public CacheService(IHallsService hallsService, IStandsService standsService, IExhibitsService exhibitsService)
        {
            _hallsService = hallsService;
            _standsService = standsService;
            _exhibitsService = exhibitsService;
        }

        public async Task CreateCache(HttpRequest httpRequest, IEnumerable<string> languageList, bool savePhotos = true)
        {
            _isPhotosSaved = !savePhotos;

            foreach(var language in languageList)
            {
                SetLanguage(httpRequest, language);
                await WriteCahce(httpRequest);
            }
        }

        private void SetLanguage(HttpRequest httpRequest, string language)
        {
            httpRequest.Headers.Remove("Accept-Language");
            httpRequest.Headers.Add("Accept-Language", language);
        }

        public async Task WriteCahce(HttpRequest httpRequest)
        {
            // Create dictionaries for saving
            var cacheText = new Dictionary<string, object>();
            var cachePhotos = new Dictionary<string, object>();
            
            var language = httpRequest.Headers["Accept-Language"];

            // Getting path for cahce saving
            var path = GetPathToTextCache(language);
            var photosCachePath = GetPathToPhotosCache();

            var halls = await _hallsService.GetAllAsync(httpRequest);
            WriteHallsAsync(halls, cacheText, cachePhotos, language);
            foreach (var hall in halls)
            {
                var hallFromDB = await _hallsService.GetAsync(httpRequest, hall.Id);
                WriteHallAsync(hallFromDB, cacheText, cachePhotos, language);
                foreach (var stand in hallFromDB.Stands)
                {
                    var standFromDB = await _standsService.GetAsync(httpRequest, hallFromDB.Id, stand.Id);
                    WriteStandAsync(standFromDB, cacheText, cachePhotos, language);
                    foreach (var exhibit in standFromDB.Exhibits)
                    {
                        var exhibitFromDB = await _exhibitsService.GetAsync(httpRequest, hallFromDB.Id, stand.Id, exhibit.Id);
                        WriteExhibitAsync(exhibitFromDB, cacheText, cachePhotos, language);
                    }
                }
            }
            var json = $"version:{GetVersion(path) + 1}{Environment.NewLine}";
            json += JsonConvert.SerializeObject(cacheText, Formatting.Indented);
            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine(json);
            };

            if (!_isPhotosSaved)
            {
                json = $"version:{GetVersion(photosCachePath) + 1}{Environment.NewLine}";
                json += JsonConvert.SerializeObject(cachePhotos, Formatting.Indented);
                using (TextWriter tw = new StreamWriter(photosCachePath))
                {
                    tw.WriteLine(json);
                };
                _isPhotosSaved = true;
            }
        }

        public Stream GetCache(string language, uint version = 0)
        {
            var path = GetPathToTextCache(language);
            if (!File.Exists(path))
            {
                throw new Error(CommonClassLibrary.Enums.Errors.Not_found, $"{language} cache not found");
            }

            if (GetVersion(path) != version)
            {
                return File.OpenRead(path);
            }
            return null;
        }

        public Stream GetCache(uint version)
        {
            var path = GetPathToPhotosCache();
            if (!File.Exists(path))
            {
                throw new Error(CommonClassLibrary.Enums.Errors.Not_found, "Photos cache not found");
            }

            if (GetVersion(path) != version)
            {
                return File.OpenRead(path);
            }
            return null;
        }

        public void WriteExhibitAsync(ExhibitDTO exhibit, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language)
        {
            // Copy bytes
            var photosBytes = new List<byte[]>();
            foreach (var photo in exhibit.Photos)
            {
                photosBytes.Add(photo.Photo);
                photo.Photo = null;
            }

            if (!_isPhotosSaved)
            {
                // Save images themselves
                cachePhotos.Add($"photo{exhibit.Id}", photosBytes);
            }

            // Save images description
            cache.Add($"{language}{exhibit.Id}description", exhibit.Photos);
            exhibit.Photos = null;

            // Save exhibit description
            cache.Add($"{language}{exhibit.Id}", exhibit);
        }

        public void WriteHallAsync(HallDTO hall, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language)
        {
            // Copy bytes
            var photosBytes = new List<byte[]>();
            foreach (var stand in hall.Stands)
            {
                photosBytes.Add(stand.Photo.Photo);
                stand.Photo.Photo = null;
            }

            if (!_isPhotosSaved)
            {
                // Write photo
                cachePhotos.Add($"photo{hall.Id}", photosBytes);
            }

            // Write text
            cache.Add($"{language}{hall.Id}", hall);
        }

        public void WriteHallsAsync(List<HallDTO> halls, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language)
        {
            List<PhotoInfoDTO> photos = new List<PhotoInfoDTO>();

            foreach (var hall in halls)
            {
                photos.Add(hall.Photo);
                hall.Photo = null;
            }

            if (!_isPhotosSaved)
            {
                // Write photo
                cachePhotos.Add("photohalls", photos);
            }

            // Write hall text
            cache.Add($"{language}halls", halls);
        }

        public void WriteStandAsync(StandDTO stand, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language)
        {
            // Save photos
            List<byte[]> photosBytes = new List<byte[]>();
            foreach (var exhibit in stand.Exhibits)
            {
                if (exhibit.Photos?.Count == 0 || exhibit.Photos == null)
                {
                    photosBytes.Add(null);
                }
                else
                {
                    photosBytes.Add(exhibit.Photos[0]?.Photo);
                    exhibit.Photos[0].Photo = null;
                }
            }

            if (!_isPhotosSaved)
            {
                // Write photos
                cachePhotos.Add($"photo{stand.Id}", photosBytes);
            }

            // Write text
            cache.Add($"{language}{stand.Id}", stand);
        }

        private string GetPathToTextCache(string language)
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += "\\cache";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += $"\\{language}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "\\cache.json";
            return path;
        }

        private string GetPathToPhotosCache()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += "\\cache";
            path += "\\cache.json";
            return path;
        }

        public uint GetVersion(string path)
        {
            uint currentVersion = 1;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    var versionLine = sr.ReadLine();
                    if (versionLine != null)
                    {
                        var versionFromFileString = versionLine.Substring(versionLine.IndexOf(':') + 1);
                        uint.TryParse(versionFromFileString, out currentVersion);
                    }
                    sr.Close();
                }
            }
            return currentVersion;
        }

        public bool IsCorrectLanguage(string language)
        {
            if (language.Equals(LanguageConstants.LanguageEn))
            {
                return true;
            }
            if (language.Equals(LanguageConstants.LanguageRu))
            {
                return true;
            }
            if (language.Equals(LanguageConstants.LanguageBy))
            {
                return true;
            }
            return false;
        }

        public bool IsCorrectLanguage(List<string> languages)
        {
            if(languages is null)
            {
                return false;
            }

            foreach(var language in languages)
            {
                if (!IsCorrectLanguage(language))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
