using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Xamarin.Forms;
using Newtonsoft.Json;
using GSU.Museum.Shared.Services;

[assembly: Dependency(typeof(ContentLoaderService))]
namespace GSU.Museum.Shared.Services
{
    public class ContentLoaderService : IContentLoaderService
    {
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

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

        public async Task<string> Load(Uri uri)
        {
            _logger.Info($"Send request to {uri}");
            try
            {
                HttpResponseMessage response = await GetHttpClient().GetAsync(uri);
                _logger.Info($"Response's status code is {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                // if server-side exception
                else if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<Error>(content);
                    _logger.Error($"Error in response: {error}");
                    throw new Error() { Info = error.Info, ErrorCode = error.ErrorCode };
                }
                // if unhandled server-side exception
                else
                {
                    _logger.Fatal("Unhandled exception");
                    throw new Exception($"response status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while sending request: {ex.Message}");
                throw ex;
            }
        }

        public async Task<Exhibit> LoadExhibit(string hallId, string standId, string id)
        {
            _logger.Info($"Loading exhibit with id {id}");
            var exhibitCached = await DependencyService.Get<CachingService>().ReadExhibitAsync(id);
            if (exhibitCached != null)
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Exhibits/{hallId}/{standId}/{id}?hash={exhibitCached.GetHashCode()}"));
                if (string.IsNullOrEmpty(content))
                {
                    return exhibitCached;
                }
                Exhibit exhibit = JsonConvert.DeserializeObject<Exhibit>(content);
                await DependencyService.Get<CachingService>().WriteExhibitAsync(exhibit);
                return exhibit;
            }
            else
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Exhibits/{hallId}/{standId}/{id}"));
                Exhibit exhibit = JsonConvert.DeserializeObject<Exhibit>(content);
                await DependencyService.Get<CachingService>().WriteExhibitAsync(exhibit);
                return exhibit;
            }
        }

        public async Task<Hall> LoadHall(string id)
        {
            _logger.Info($"Loading hall with id {id}");
            var hallCached = await DependencyService.Get<CachingService>().ReadHallAsync(id);
            if (hallCached != null)
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Halls/{id}?hash={hallCached.GetHashCode()}"));
                if (string.IsNullOrEmpty(content))
                {
                    return hallCached;
                }
                Hall hall = JsonConvert.DeserializeObject<Hall>(content);
                await DependencyService.Get<CachingService>().WriteHallAsync(hall);
                return hall;
            }
            else
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Halls/{id}"));
                Hall hall = JsonConvert.DeserializeObject<Hall>(content);
                await DependencyService.Get<CachingService>().WriteHallAsync(hall);
                return hall;
            }
        }

        public async Task<List<Hall>> LoadHalls()
        {
            _logger.Info($"Loading halls");
            var hallsCached = await DependencyService.Get<CachingService>().ReadHallsAsync();
            if (hallsCached != null)
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Halls?hash={GetHash(hallsCached)}"));
                if (string.IsNullOrEmpty(content))
                {
                    return hallsCached;
                }
                List<Hall> halls = JsonConvert.DeserializeObject<List<Hall>>(content);
                await DependencyService.Get<CachingService>().WriteHallsAsync(halls);
                return halls;
            }
            else
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Halls"));
                List<Hall> halls = JsonConvert.DeserializeObject<List<Hall>>(content);
                await DependencyService.Get<CachingService>().WriteHallsAsync(halls);
                return halls;
            }
        }

        public async Task<Stand> LoadStand(string hallId, string id)
        {
            _logger.Info($"Loading stand with id {id}");
            var standCached = await DependencyService.Get<CachingService>().ReadStandAsync(id);
            if (standCached != null)
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Stands/{hallId}/{id}?hash={standCached.GetHashCode()}"));
                if (string.IsNullOrEmpty(content))
                {
                    return standCached;
                }
                Stand stand = JsonConvert.DeserializeObject<Stand>(content);
                await DependencyService.Get<CachingService>().WriteStandAsync(stand);
                return stand;
            }
            else
            {
                string content = await Load(new Uri($"https://{App.UriBase}/api/Stands/{hallId}/{id}"));
                Stand stand = JsonConvert.DeserializeObject<Stand>(content); 
                await DependencyService.Get<CachingService>().WriteStandAsync(stand);
                return stand;
            }
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
