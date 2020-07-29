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
                _logger.Info($"Read from cache exhibit {language}-{id}");
                ExhibitDTO exhibit = await BlobCache.LocalMachine.GetObject<ExhibitDTO>($"{language}{id}");
                exhibit.Photos = await BlobCache.LocalMachine.GetObject<List<PhotoInfoDTO>>(id);
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
                _logger.Info($"Read from cache hall {language}-{id}");
                HallDTO hall = await BlobCache.LocalMachine.GetObject<HallDTO>($"{language}{id}");
                hall.Photo = await BlobCache.LocalMachine.GetObject<PhotoInfoDTO>(id);
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
                _logger.Info($"Read from halls");
                List<HallDTO> halls = await BlobCache.LocalMachine.GetObject<List<HallDTO>>($"{language}halls");
                return halls;
            }
            return null;
        }

        public async Task<Settings> ReadSettings()
        {
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains("settings"))
            {
                _logger.Info($"Read from cache settings");
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
                _logger.Info($"Read from cache stand {language}-{id}");
                StandDTO stand = await BlobCache.LocalMachine.GetObject<StandDTO>($"{language}{id}");
                stand.Photo = await BlobCache.LocalMachine.GetObject<PhotoInfoDTO>(id);
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

            _logger.Info($"Write to cache ExhibitDTO {language}-{exhibit.Id}");

            await BlobCache.LocalMachine.InsertObject(exhibit.Id, exhibit.Photos);
            List<PhotoInfoDTO> photos = exhibit.Photos;
            exhibit.Photos = null;
            await BlobCache.LocalMachine.InsertObject($"{language}{exhibit.Id}", exhibit);
            exhibit.Photos = photos;
        }

        public async Task WriteHallAsync(HallDTO hall)
        {
            if (!App.Settings.UseCache)
            {
                return;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write to cache HallDTO {language}-{hall.Id}");

            await BlobCache.LocalMachine.InsertObject(hall.Id, hall.Photo);
            PhotoInfoDTO photo = hall.Photo;
            hall.Photo = null;
            await BlobCache.LocalMachine.InsertObject($"{language}{hall.Id}", hall);
            hall.Photo = photo;
        }

        public async Task WriteHallsAsync(List<HallDTO> halls)
        {
            if (!App.Settings.UseCache)
            {
                return;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write to cache halls");

            await BlobCache.LocalMachine.InsertObject($"{language}halls", halls);
        }

        public async Task WriteSettings()
        {
            _logger.Info($"Write to cache settings");
            await BlobCache.LocalMachine.InsertObject("settings", App.Settings);
        }

        public async Task WriteStandAsync(StandDTO stand)
        {
            if (!App.Settings.UseCache)
            {
                return;
            }

            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write to cache StandDTO {language}-{stand.Id}");

            await BlobCache.LocalMachine.InsertObject(stand.Id, stand.Photo);
            PhotoInfoDTO photo = stand.Photo;
            stand.Photo = null;
            await BlobCache.LocalMachine.InsertObject($"{language}{stand.Id}", stand);
            stand.Photo = photo;
        }
    }
}
