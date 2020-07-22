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


[assembly: Dependency(typeof(CachingService))]
namespace GSU.Museum.Shared.Services
{
    public class CachingService : ICachingService
    {
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<Exhibit> ReadExhibitAsync(string id)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}{id}"))
            {
                _logger.Info($"Read from cache exhibit {language}-{id}");
                Exhibit exhibit = await BlobCache.LocalMachine.GetObject<Exhibit>($"{language}{id}");
                exhibit.Photos = await BlobCache.LocalMachine.GetObject<List<PhotoInfo>>(id);
                return exhibit;
            }
            return null;
        }

        public async Task<Hall> ReadHallAsync(string id)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}{id}"))
            {
                _logger.Info($"Read from cache hall {language}-{id}");
                Hall hall = await BlobCache.LocalMachine.GetObject<Hall>($"{language}{id}");
                hall.Photo = await BlobCache.LocalMachine.GetObject<PhotoInfo>(id);
                return hall;
            }
            return null;
        }

        public async Task<List<Hall>> ReadHallsAsync()
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}halls"))
            {
                _logger.Info($"Read from halls");
                List<Hall> halls = await BlobCache.LocalMachine.GetObject<List<Hall>>($"{language}halls");
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

        public async Task<Stand> ReadStandAsync(string id)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            if (keys.Contains($"{language}{id}"))
            {
                _logger.Info($"Read from cache stand {language}-{id}");
                Stand stand = await BlobCache.LocalMachine.GetObject<Stand>($"{language}{id}");
                stand.Photo = await BlobCache.LocalMachine.GetObject<PhotoInfo>(id);
                return stand;
            }
            return null;
        }

        public async Task WriteExhibitAsync(Exhibit exhibit)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write to cache Exhibit {language}-{exhibit.Id}");

            await BlobCache.LocalMachine.InsertObject(exhibit.Id, exhibit.Photos);
            List<PhotoInfo> photos = exhibit.Photos;
            exhibit.Photos = null;
            await BlobCache.LocalMachine.InsertObject($"{language}{exhibit.Id}", exhibit);
            exhibit.Photos = photos;
        }

        public async Task WriteHallAsync(Hall hall)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write to cache Hall {language}-{hall.Id}");

            await BlobCache.LocalMachine.InsertObject(hall.Id, hall.Photo);
            PhotoInfo photo = hall.Photo;
            hall.Photo = null;
            await BlobCache.LocalMachine.InsertObject($"{language}{hall.Id}", hall);
            hall.Photo = photo;
        }

        public async Task WriteHallsAsync(List<Hall> halls)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write to cache halls");

            await BlobCache.LocalMachine.InsertObject($"{language}halls", halls);
        }

        public async Task WriteSettings()
        {
            _logger.Info($"Write to cache settings");
            await BlobCache.LocalMachine.InsertObject("settings", App.Settings);
        }

        public async Task WriteStandAsync(Stand stand)
        {
            string language = Thread.CurrentThread.CurrentUICulture.Name;

            _logger.Info($"Write to cache Stand {language}-{stand.Id}");

            await BlobCache.LocalMachine.InsertObject(stand.Id, stand.Photo);
            PhotoInfo photo = stand.Photo;
            stand.Photo = null;
            await BlobCache.LocalMachine.InsertObject($"{language}{stand.Id}", stand);
            stand.Photo = photo;
        }
    }
}
