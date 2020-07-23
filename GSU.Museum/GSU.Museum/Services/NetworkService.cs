using GSU.Museum.Shared.Data.Enums;
using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                    language = "Ru";
                    break;
                case "en-US":
                    language = "En";
                    break;
                case "be-BY":
                    language = "Be";
                    break;
                default:
                    language = "En";
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

            httpClient.DefaultRequestHeaders.Add("Language", language);
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", "U3VwZXJTZWNyZXRBcGlLZXkxMjM");
            return httpClient;
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

        public async Task<Exhibit> LoadExhibitAsync(string hallId, string standId, string id)
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Exhibits/{hallId}/{standId}/{id}"));
                Exhibit exhibit = JsonConvert.DeserializeObject<Exhibit>(content);
                await DependencyService.Get<CachingService>().WriteExhibitAsync(exhibit);
                return exhibit;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild};
        }

        public async Task<Exhibit> LoadExhibitAsync(string hallId, string standId, string id, Exhibit exhibitCached)
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
                    Exhibit exhibit = JsonConvert.DeserializeObject<Exhibit>(content);
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

        public async Task<Hall> LoadHallAsync(string id)
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Halls/{id}"));
                Hall hall = JsonConvert.DeserializeObject<Hall>(content);
                await DependencyService.Get<CachingService>().WriteHallAsync(hall);
                return hall;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild };
        }

        public async Task<Hall> LoadHallAsync(string id, Hall hallCached)
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
                    Hall hall = JsonConvert.DeserializeObject<Hall>(content);
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

        public async Task<List<Hall>> LoadHallsAsync()
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Halls"));
                List<Hall> halls = JsonConvert.DeserializeObject<List<Hall>>(content);
                await DependencyService.Get<CachingService>().WriteHallsAsync(halls);
                return halls;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild };
        }

        public async Task<List<Hall>> LoadHallsAsync(List<Hall> hallsCached)
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
                    List<Hall> halls = JsonConvert.DeserializeObject<IEnumerable<Hall>>(content).ToList();
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

        public async Task<Stand> LoadStandAsync(string hallId, string id)
        {
            if (CheckConnection())
            {
                string content = await LoadAsync(new Uri($"https://{App.UriBase}/api/Stands/{hallId}/{id}"));
                Stand stand = JsonConvert.DeserializeObject<Stand>(content);
                await DependencyService.Get<CachingService>().WriteStandAsync(stand);
                return stand;
            }
            throw new Error() { ErrorCode = Errors.Failed_Connection, Info = AppResources.ErrorMessage_LoadingFaild };
        }

        public async Task<Stand> LoadStandAsync(string hallId, string id, Stand standCached)
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
                    Stand stand = JsonConvert.DeserializeObject<Stand>(content);
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

        public int GetHash(List<Hall> hallsCached)
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
    }
}
