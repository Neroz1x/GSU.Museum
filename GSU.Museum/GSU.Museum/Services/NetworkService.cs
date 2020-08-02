using Akavache;
using Akavache.Sqlite3;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(NetworkService))]
namespace GSU.Museum.Shared.Services
{
    public class NetworkService : INetworkService
    {
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Check Internet connection
        /// </summary>
        /// <returns></returns>
        private bool CheckConnection()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet || Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public HttpClient GetHttpClient()
        {
            string language;
            switch (Thread.CurrentThread.CurrentUICulture.Name)
            {
                case "ru-RU":
                    language = "ru";
                    break;
                case "en-US":
                    language = "en";
                    break;
                case "be-BY":
                    language = "be";
                    break;
                default:
                    language = "en";
                    break;
            }
            HttpClient httpClient;

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    httpClient = new HttpClient(DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler());
                    break;
                default:
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    httpClient = new HttpClient(new HttpClientHandler());
                    break;
            }

            httpClient.DefaultRequestHeaders.Add("Accept-Language", language);
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", "U3VwZXJTZWNyZXRBcGlLZXkxMjM");
            return httpClient;
        }

        public async Task<Stream> LoadStreamAsync(Uri uri)
        {
            _logger.Info($"Send request to {uri}");
            try
            {
                HttpResponseMessage response = await GetHttpClient().GetAsync(uri);
                _logger.Info($"Response's status code is {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    if(content.Length == 0)
                    {
                        throw new Error() { Info = AppResources.ErrorMessage_CacheIsUpToDate, ErrorCode = Errors.Info };
                    }
                    return content;
                }
                // if server-side exception
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var error = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
                    _logger.Error($"Error in response: {error}");
                    throw new Error() { Info = error.Info, ErrorCode = error.ErrorCode };
                }
                else if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Error() { Info = AppResources.ErrorMessage_CanNotLoadCache, ErrorCode = Errors.Not_found };
                }
                // if unhandled server-side exception
                else
                {
                    _logger.Fatal("Unhandled exception");
                    throw new Exception($"response status code: {response.StatusCode};");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while sending request: {ex.Message}");
                throw ex;
            }
        }

        public async Task<string> LoadAsync(Uri uri)
        {
            _logger.Info($"Send request to {uri}");
            try
            {
                HttpResponseMessage response = await GetHttpClient().GetAsync(uri);
                _logger.Info($"Response's status code is {response.StatusCode}");
                string content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return content;
                }
                // if server-side exception
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var error = JsonConvert.DeserializeObject<Error>(content);
                    _logger.Error($"Error in response: {error}");
                    throw new Error() { Info = error.Info, ErrorCode = error.ErrorCode };
                }
                // if unhandled server-side exception
                else
                {
                    _logger.Fatal("Unhandled exception");
                    throw new Exception($"response status code: {response.StatusCode}; content: {content}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while sending request: {ex.Message}");
                throw ex;
            }
        }

        public async Task<ExhibitDTO> LoadExhibitAsync(string hallId, string standId, string id)
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Exhibits/{hallId}/{standId}/{id}"));
                ExhibitDTO exhibit = JsonConvert.DeserializeObject<ExhibitDTO>(content);
                await DependencyService.Get<CachingService>().WriteExhibitAsync(exhibit);
                return exhibit;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild};
        }

        public async Task<ExhibitDTO> LoadExhibitAsync(string hallId, string standId, string id, ExhibitDTO exhibitCached)
        {
            if (CheckConnection())
            {
                try
                {
                    string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Exhibits/{hallId}/{standId}/{id}?hash={exhibitCached.GetHashCode()}"));
                    if (string.IsNullOrEmpty(content))
                    {
                        return exhibitCached;
                    }
                    ExhibitDTO exhibit = JsonConvert.DeserializeObject<ExhibitDTO>(content);
                    await DependencyService.Get<CachingService>().WriteExhibitAsync(exhibit);
                    return exhibit;
                }
                catch (Exception ex)
                {
                    if (ex is HttpRequestException || ex is WebException)
                    {
                        return exhibitCached;
                    }
                }   
            }
            return exhibitCached;
        }

        public async Task<HallDTO> LoadHallAsync(string id)
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Halls/{id}"));
                HallDTO hall = JsonConvert.DeserializeObject<HallDTO>(content);
                await DependencyService.Get<CachingService>().WriteHallAsync(hall);
                return hall;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild };
        }

        public async Task<HallDTO> LoadHallAsync(string id, HallDTO hallCached)
        {
            if (CheckConnection())
            {
                try
                {
                    string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Halls/{id}?hash={hallCached.GetHashCode()}"));
                    if (string.IsNullOrEmpty(content))
                    {
                        return hallCached;
                    }
                    HallDTO hall = JsonConvert.DeserializeObject<HallDTO>(content);
                    await DependencyService.Get<CachingService>().WriteHallAsync(hall);
                    return hall;
                }
                catch (Exception ex)
                {
                    if (ex is HttpRequestException || ex is WebException)
                    {
                        return hallCached;
                    }
                }
            }
            return hallCached;
        }

        public async Task<List<HallDTO>> LoadHallsAsync()
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Halls"));
                List<HallDTO> halls = JsonConvert.DeserializeObject<List<HallDTO>>(content);
                await DependencyService.Get<CachingService>().WriteHallsAsync(halls);
                return halls;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild };
        }

        public async Task<List<HallDTO>> LoadHallsAsync(List<HallDTO> hallsCached)
        {
            if (CheckConnection())
            {
                try
                {
                    string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Halls?hash={GetHash(hallsCached)}"));
                    if (string.IsNullOrEmpty(content))
                    {
                        return hallsCached;
                    }
                    List<HallDTO> halls = JsonConvert.DeserializeObject<IEnumerable<HallDTO>>(content).ToList();
                    await DependencyService.Get<CachingService>().WriteHallsAsync(halls);
                    return halls;
                }
                catch (Exception ex)
                {
                    if (ex is HttpRequestException || ex is WebException)
                    {
                        return hallsCached;
                    }
                }
            }
            return hallsCached;
        }

        public async Task<StandDTO> LoadStandAsync(string hallId, string id)
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Stands/{hallId}/{id}"));
                StandDTO stand = JsonConvert.DeserializeObject<StandDTO>(content);
                await DependencyService.Get<CachingService>().WriteStandAsync(stand);
                return stand;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild };
        }

        public async Task<StandDTO> LoadStandAsync(string hallId, string id, StandDTO standCached)
        {
            if (CheckConnection())
            {
                try
                {
                    string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Stands/{hallId}/{id}?hash={standCached.GetHashCode()}"));
                    if (string.IsNullOrEmpty(content))
                    {
                        return standCached;
                    }
                    StandDTO stand = JsonConvert.DeserializeObject<StandDTO>(content);
                    await DependencyService.Get<CachingService>().WriteStandAsync(stand);
                    return stand;
                }
                catch (Exception ex)
                {
                    if (ex is HttpRequestException || ex is WebException)
                    {
                        return standCached;
                    }
                }
            }
            return standCached;
            
        }

        public int GetHash(List<HallDTO> hallsCached)
        {
            unchecked
            {
                int hash = (int)2166136261;
                foreach (var hallCached in hallsCached)
                {
                    hash = (hash * 16777619) ^ (hallCached?.GetHashCode() ?? 1);
                }
                return hash;
            }
        }

        public async Task LoadCacheAsync()
        {
            if (CheckConnection())
            {
                var path = "/data/user/0/com.companyname.gsu.museum/cache";
                DependencyService.Get<CachingService>().WriteCache(await LoadStreamAsync(new Uri($"https://{App.UriBase}/api/Cache/GetDB")), path + "/blobs.db");
                DependencyService.Get<CachingService>().WriteCache(await LoadStreamAsync(new Uri($"https://{App.UriBase}/api/Cache/GetDBSHM")), path + "/blobs.db-shm");
                DependencyService.Get<CachingService>().WriteCache(await LoadStreamAsync(new Uri($"https://{App.UriBase}/api/Cache/GetDBWAL")), path + "/blobs.db-wal");
                await DependencyService.Get<CachingService>().WriteSettings();
                return;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild };
        }
    }
}
