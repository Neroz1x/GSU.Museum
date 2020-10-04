using Akavache;
using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Xamarin.Forms;
using System.Threading.Tasks;
using GSU.Museum.CommonClassLibrary.Models;
using System.IO;

[assembly: Dependency(typeof(CachingService))]
namespace GSU.Museum.Shared.Services
{
    public class CachingService : ICachingService
    {
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<ExhibitDTO> ReadExhibitAsync(string id)
        {
            if (!App.Settings.UseCache) 
            { 
                return null; 
            }
            
            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}{id}"))
            {
                _logger.Info($"Read exhibit {language}-{id} from cache");
                
                // Read exhibit
                ExhibitDTO exhibit = await BlobCache.LocalMachine.GetObject<ExhibitDTO>($"{language}{id}");

                // Read photos description
                exhibit.Photos = await BlobCache.LocalMachine.GetObject<List<PhotoInfoDTO>>($"{language}{id}description");

                // Read photos
                var photosBytes = await BlobCache.LocalMachine.GetObject<List<byte[]>>(id);
                int index = 0;
                foreach(var photo in photosBytes)
                {
                    exhibit.Photos[index++].Photo = photo;
                }
                return exhibit;
            }
            return null;
        }

        public async Task<HallDTO> ReadHallAsync(string id)
        {
            if (!App.Settings.UseCache)
            {
                return null;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}{id}"))
            {
                _logger.Info($"Read hall {language}-{id} from cache");

                // Read hall
                HallDTO hall = await BlobCache.LocalMachine.GetObject<HallDTO>($"{language}{id}");

                // Read photo
                var photos = await BlobCache.LocalMachine.GetObject<List<byte[]>>(id);
                for(int i = 0; i < hall.Stands.Count; i++)
                {
                    hall.Stands[i].Photo.Photo = photos[i];
                }
                return hall;
            }
            return null;
        }

        public async Task<List<HallDTO>> ReadHallsAsync()
        {
            if (!App.Settings.UseCache)
            {
                return null;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}halls"))
            {
                _logger.Info($"Read halls from");

                // Read halls
                List<HallDTO> halls = await BlobCache.LocalMachine.GetObject<List<HallDTO>>($"{language}halls");
                
                // Read photos
                List<PhotoInfoDTO> photos = await BlobCache.LocalMachine.GetObject<List<PhotoInfoDTO>>($"{language}halls-photo");
                for(int i = 0; i < halls.Count; i++)
                {
                    halls[i].Photo = photos[i];
                }
                return halls;
            }
            return null;
        }

        public async Task<Settings> ReadSettings()
        {
            await BlobCache.LocalMachine.InvalidateAll();
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains("settings"))
            {
                _logger.Info($"Read settings from cache");
                Settings settings = await BlobCache.LocalMachine.GetObject<Settings>("settings");
                return settings;
            }
            return new Settings();
        }

        public async Task<StandDTO> ReadStandAsync(string id)
        {
            if (!App.Settings.UseCache)
            {
                return null;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}{id}"))
            {
                _logger.Info($"Read stand {language}-{id} from cache");
                // Read stand
                StandDTO stand = await BlobCache.LocalMachine.GetObject<StandDTO>($"{language}{id}");
                // Read photo
                var photosBytes = await BlobCache.LocalMachine.GetObject<List<byte[]>>(id);
                for(int i = 0; i < photosBytes.Count; i++)
                {
                    stand.Exhibits[i].Photos[0].Photo = photosBytes[i];
                }
                return stand;
            }
            return null;
        }

        public async Task WriteExhibitAsync(ExhibitDTO exhibit)
        {
            if (!App.Settings.UseCache)
            {
                return;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write exhibit {language}-{exhibit.Id} to cache");

            // Copy photos
            List<PhotoInfoDTO> photos = new List<PhotoInfoDTO>(exhibit.Photos);

            // Copy bytes
            var photosBytes = new List<byte[]>();
            foreach(var photo in exhibit.Photos)
            {
                photosBytes.Add(photo.Photo);
                photo.Photo = null;
            }

            // Save images themselves
            await BlobCache.LocalMachine.InsertObject(exhibit.Id, photosBytes);
            
            // Save images description
            await BlobCache.LocalMachine.InsertObject($"{language}{exhibit.Id}description", exhibit.Photos);
            exhibit.Photos = null;

            // Save exhibit description
            await BlobCache.LocalMachine.InsertObject($"{language}{exhibit.Id}", exhibit);

            for(int i = 0; i < photos.Count; i++)
            {
                photos[i].Photo = photosBytes[i];
            }
            exhibit.Photos = photos;
        }

        public async Task WriteHallAsync(HallDTO hall)
        {
            if (!App.Settings.UseCache)
            {
                return;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write hall {language}-{hall.Id} to cache");

            // Copy bytes
            var photosBytes = new List<byte[]>();
            foreach (var stand in hall.Stands)
            {
                photosBytes.Add(stand.Photo.Photo);
                stand.Photo.Photo = null;
            }

            // Write photo
            await BlobCache.LocalMachine.InsertObject(hall.Id, photosBytes);
            
            // Write text
            await BlobCache.LocalMachine.InsertObject($"{language}{hall.Id}", hall);

            for (int i = 0; i < photosBytes.Count; i++)
            {
                hall.Stands[i].Photo.Photo = photosBytes[i];
            }
        }

        public async Task WriteHallsAsync(List<HallDTO> halls)
        {
            if (!App.Settings.UseCache)
            {
                return;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write halls to cache");

            List<PhotoInfoDTO> photos = new List<PhotoInfoDTO>();

            foreach(var hall in halls)
            {
                photos.Add(hall.Photo);
                hall.Photo = null;
            }

            // Write photo
            await BlobCache.LocalMachine.InsertObject($"{language}halls-photo", photos);
            
            // Write hall text
            await BlobCache.LocalMachine.InsertObject($"{language}halls", halls);

            for(int i = 0; i < photos.Count; i++)
            {
                halls[i].Photo = photos[i];
            }
        }

        public async Task WriteSettings()
        {
            _logger.Info($"Write settings to cache");
            await BlobCache.LocalMachine.InsertObject("settings", App.Settings);
        }

        public async Task WriteStandAsync(StandDTO stand)
        {
            if (!App.Settings.UseCache)
            {
                return;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write stand {language}-{stand.Id} to cache");

            // Save photos
            List<byte[]> photosBytes = new List<byte[]>();
            foreach(var exhibit in stand.Exhibits)
            {
                photosBytes.Add(exhibit.Photos[0]?.Photo);
                exhibit.Photos[0].Photo = null;
            }

            // Write photos
            await BlobCache.LocalMachine.InsertObject(stand.Id, photosBytes);
            
            // Write text
            await BlobCache.LocalMachine.InsertObject($"{language}{stand.Id}", stand);

            for(int i = 0; i < photosBytes.Count; i++)
            {
                stand.Exhibits[i].Photos[0].Photo = photosBytes[i];
            }
        }

        // TODO: Find out another way to replace cahce
        public void WriteCache(Stream stream, string path)
        {
            File.Delete(path);
            using (FileStream outputFileStream = new FileStream(path, FileMode.CreateNew))
            {
                stream.CopyTo(outputFileStream);
            }
        }

        public async Task ClearCache()
        {
            await BlobCache.LocalMachine.InvalidateAll();
            await WriteSettings();
        }
    }
}
