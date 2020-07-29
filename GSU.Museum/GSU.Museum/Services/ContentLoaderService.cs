using GSU.Museum.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using GSU.Museum.Shared.Services;
using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Resources;

[assembly: Dependency(typeof(ContentLoaderService))]
namespace GSU.Museum.Shared.Services
{
    public class ContentLoaderService : IContentLoaderService
    {
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<ExhibitDTO> LoadExhibitAsync(string hallId, string standId, string id)
        {
            _logger.Info($"Loading exhibit with id {id}");
            var exhibitCached = await DependencyService.Get<CachingService>().ReadExhibitAsync(id);
            if (exhibitCached != null)
            {
                if (App.Settings.CheckForUpdates)
                {
                    return await DependencyService.Get<NetworkService>().LoadExhibitAsync(hallId, standId, id, exhibitCached);
                }
                return exhibitCached;
            }
            else
            {
                if (App.Settings.UseCache && App.Settings.UseOnlyCache)
                {
                    throw new Error(){ ErrorCode = CommonClassLibrary.Enums.Errors.Not_Found_In_Cache, Info = AppResources.ErrorMessage_NotFoundInCache };
                }
                return await DependencyService.Get<NetworkService>().LoadExhibitAsync(hallId, standId, id);
            }
        }

        public async Task<HallDTO> LoadHallAsync(string id)
        {
            _logger.Info($"Loading hall with id {id}");
            var hallCached = await DependencyService.Get<CachingService>().ReadHallAsync(id);
            if (hallCached != null)
            {
                if (App.Settings.CheckForUpdates)
                {
                    return await DependencyService.Get<NetworkService>().LoadHallAsync(id, hallCached);
                }
                return hallCached;
            }
            else
            {
                if (App.Settings.UseCache && App.Settings.UseOnlyCache)
                {
                    throw new Error() { ErrorCode = CommonClassLibrary.Enums.Errors.Not_Found_In_Cache, Info = AppResources.ErrorMessage_NotFoundInCache };
                }
                return await DependencyService.Get<NetworkService>().LoadHallAsync(id);
            }
        }

        public async Task<List<HallDTO>> LoadHallsAsync()
        {
            _logger.Info($"Loading halls");
            var hallsCached = await DependencyService.Get<CachingService>().ReadHallsAsync();
            if (hallsCached != null)
            {
                if (App.Settings.CheckForUpdates)
                {
                    return await DependencyService.Get<NetworkService>().LoadHallsAsync(hallsCached);
                }
                return hallsCached;
            }
            else
            {
                if (App.Settings.UseCache && App.Settings.UseOnlyCache)
                {
                    throw new Error() { ErrorCode = CommonClassLibrary.Enums.Errors.Not_Found_In_Cache, Info = AppResources.ErrorMessage_NotFoundInCache };
                }
                return await DependencyService.Get<NetworkService>().LoadHallsAsync();
            }
        }

        public async Task<StandDTO> LoadStandAsync(string hallId, string id)
        {
            _logger.Info($"Loading stand with id {id}");
            var standCached = await DependencyService.Get<CachingService>().ReadStandAsync(id);
            if (standCached != null)
            {
                if (App.Settings.CheckForUpdates)
                {
                    return await DependencyService.Get<NetworkService>().LoadStandAsync(hallId, id, standCached);
                }
                return standCached;
            }
            else
            {
                if (App.Settings.UseCache && App.Settings.UseOnlyCache)
                {
                    throw new Error() { ErrorCode = CommonClassLibrary.Enums.Errors.Not_Found_In_Cache, Info = AppResources.ErrorMessage_NotFoundInCache };
                }
                return await DependencyService.Get<NetworkService>().LoadStandAsync(hallId, id);
            }
        }
    }
}
