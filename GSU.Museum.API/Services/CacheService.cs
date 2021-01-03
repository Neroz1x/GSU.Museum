using Akavache.Sqlite3;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GSU.Museum.API.Services
{
    public class CacheService : ICacheService
    {
        private readonly IHallsService _hallsService;
        private readonly IStandsService _standsService;
        private readonly IExhibitsService _exhibitsService;
        private StringValues _language;
        private SqlRawPersistentBlobCache _blobCache;
        private const string VersionKey = "version";

        public CacheService(IHallsService hallsService, IStandsService standsService, IExhibitsService exhibitsService)
        {
            _hallsService = hallsService;
            _standsService = standsService;
            _exhibitsService = exhibitsService;
        }

        public FileStream GetCahceDB(HttpRequest httpRequest)
        {
            SetLanguage(httpRequest);

            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += "\\cache";
            path += $"\\{_language}";
            if (Directory.Exists(path))
            {
                return File.OpenRead(path + "\\blobs.db");
            }
            return null;
        }

        public async Task CreateCache(HttpRequest httpRequest)
        {
            CreateBlob(httpRequest);
            
            var keys = await _blobCache.GetAllKeys();
            uint version = 1;
            if (keys.Contains(VersionKey))
            {
                version = await _blobCache.GetObject<uint>(VersionKey);
                version++;
            }

            await _blobCache.InsertObject<uint>(VersionKey, version);
            
            var halls = await _hallsService.GetAllAsync(httpRequest);
            await WriteHallsAsync(halls);
            foreach(var hall in halls)
            {
                var hallFromDB = await _hallsService.GetAsync(httpRequest, hall.Id);
                await WriteHallAsync(hallFromDB);
                foreach(var stand in hallFromDB.Stands)
                {
                    var standFromDB = await _standsService.GetAsync(httpRequest, hallFromDB.Id, stand.Id);
                    await WriteStandAsync(standFromDB);
                    foreach(var exhibit in standFromDB.Exhibits)
                    {
                        var exhibitFromDB = await _exhibitsService.GetAsync(httpRequest, hallFromDB.Id, stand.Id, exhibit.Id);
                        await WriteExhibitAsync(exhibitFromDB);
                    }
                }
            }
            await _blobCache.Flush();
        }

        public async Task WriteExhibitAsync(ExhibitDTO exhibit)
        {
            await _blobCache.InsertObject(exhibit.Id, exhibit.Photos);
            List<PhotoInfoDTO> photos = exhibit.Photos;
            exhibit.Photos = null;
            await _blobCache.InsertObject($"{_language}{exhibit.Id}", exhibit);
            exhibit.Photos = photos;
        }

        public async Task WriteHallAsync(HallDTO hall)
        {
            await _blobCache.InsertObject(hall.Id, hall.Photo);
            PhotoInfoDTO photo = hall.Photo;
            hall.Photo = null;
            await _blobCache.InsertObject($"{_language}{hall.Id}", hall);
            hall.Photo = photo;
        }

        public async Task WriteHallsAsync(List<HallDTO> halls)
        {
            await _blobCache.InsertObject($"{_language}halls", halls);
        }

        public async Task WriteStandAsync(StandDTO stand)
        {
            await _blobCache.InsertObject(stand.Id, stand.Photo);
            PhotoInfoDTO photo = stand.Photo;
            stand.Photo = null;
            await _blobCache.InsertObject($"{_language}{stand.Id}", stand);
            stand.Photo = photo;
        }

        private void SetLanguage(HttpRequest httpRequest)
        {
            if (httpRequest != null)
            {
                if (!httpRequest.Headers.TryGetValue("Accept-Language", out _language))
                {
                    _language = "en-US";
                }
            }
            else
            {
                _language = "en-US";
            }
            switch (_language)
            {
                case "ru":
                    _language = "ru-RU";
                    break;
                case "en":
                    _language = "en-US";
                    break;
                case "be":
                    _language = "be-BY";
                    break;
                default:
                    _language = "en-US";
                    break;
            }
        }

        public FileStream GetCahceDBSHM(HttpRequest httpRequest)
        {
            SetLanguage(httpRequest);

            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += "\\cache";
            path += $"\\{_language}";
            if (Directory.Exists(path))
            {
                return File.OpenRead(path + "\\blobs.db-shm");
            }
            return null;
        }

        public FileStream GetCahceDBWAL(HttpRequest httpRequest)
        {
            SetLanguage(httpRequest);

            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += "\\cache";
            path += $"\\{_language}";
            if (Directory.Exists(path))
            {
                return File.OpenRead(path + "\\blobs.db-wal");
            }
            return null;
        }

        private void CreateBlob(HttpRequest httpRequest)
        {
            SetLanguage(httpRequest);
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += "\\cache";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += $"\\{_language}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _blobCache = new SqlRawPersistentBlobCache(path + "\\blobs.db");
        }

        public async Task<bool> IsUpToDate(int version, HttpRequest httpRequest)
        {
            CreateBlob(httpRequest);

            var keys = await _blobCache.GetAllKeys();
            uint currentVersion = 0;
            if (keys.Contains(VersionKey))
            {
                currentVersion = await _blobCache.GetObject<uint>(VersionKey);
                
                keys = await _blobCache.GetAllKeys();
                if (currentVersion == version)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
