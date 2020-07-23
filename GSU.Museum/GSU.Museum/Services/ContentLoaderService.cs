using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using GSU.Museum.Shared.Services;

[assembly: Dependency(typeof(ContentLoaderService))]
namespace GSU.Museum.Shared.Services
{
    public class ContentLoaderService : IContentLoaderService
    {
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<Exhibit> LoadExhibitAsync(string hallId, string standId, string id)
        {
            _logger.Info($"Loading exhibit with id {id}");
            var exhibitCached = await DependencyService.Get<CachingService>().ReadExhibitAsync(id);
            if (exhibitCached != null)
            {
                return await DependencyService.Get<NetworkService>().LoadExhibitAsync(hallId, standId, id, exhibitCached);
            }
            else
            {
                return await DependencyService.Get<NetworkService>().LoadExhibitAsync(hallId, standId, id);
            }
        }

        public async Task<Hall> LoadHallAsync(string id)
        {
            _logger.Info($"Loading hall with id {id}");
            var hallCached = await DependencyService.Get<CachingService>().ReadHallAsync(id);
            if (hallCached != null)
            {
                return await DependencyService.Get<NetworkService>().LoadHallAsync(id, hallCached);
            }
            else
            {
                return await DependencyService.Get<NetworkService>().LoadHallAsync(id);
            }
        }

        public async Task<List<Hall>> LoadHallsAsync()
        {
            _logger.Info($"Loading halls");
            var hallsCached = await DependencyService.Get<CachingService>().ReadHallsAsync();
            if (hallsCached != null)
            {
                return await DependencyService.Get<NetworkService>().LoadHallsAsync(hallsCached);
            }
            else
            {
                return await DependencyService.Get<NetworkService>().LoadHallsAsync();
            }
        }

        public async Task<Stand> LoadStandAsync(string hallId, string id)
        {
            _logger.Info($"Loading stand with id {id}");
            var standCached = await DependencyService.Get<CachingService>().ReadStandAsync(id);
            if (standCached != null)
            {
                return await DependencyService.Get<NetworkService>().LoadStandAsync(hallId, id, standCached);
            }
            else
            {
                return await DependencyService.Get<NetworkService>().LoadStandAsync(hallId, id);
            }
        }
    }
}
