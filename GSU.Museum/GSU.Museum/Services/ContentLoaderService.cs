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

        private string _language;

        public ContentLoaderService()
        {
            switch (Thread.CurrentThread.CurrentUICulture.Name)
            {
                case "ru-RU":
                    _language = "Ru";
                    break;
                case "en-US":
                    _language = "En";
                    break;
                case "be-BY":
                    _language = "Be";
                    break;
                default:
                    _language = "En";
                    break;
            }
        }

        public HttpClient GetHttpClient()
        {
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

            httpClient.DefaultRequestHeaders.Add("Language", _language);
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", "U3VwZXJTZWNyZXRBcGlLZXkxMjM");
            return httpClient;
        }

        public async Task<Exhibit> LoadExhibit(string hallId, string standId, string id)
        {
            _logger.Info($"Loading exhibit with id {id}");
            string content = await Load(new Uri($"https://{App.UriBase}/api/Exhibits/{hallId}/{standId}/{id}"));
            Exhibit exhibit = JsonConvert.DeserializeObject<Exhibit>(content);
            return exhibit;
        }

        public async Task<Hall> LoadHall(string id)
        {
            _logger.Info($"Loading hallwith id {id}");
            string content = await Load(new Uri($"https://{App.UriBase}/api/Halls/{id}"));
            Hall hall = JsonConvert.DeserializeObject<Hall>(content);
            return hall;
        }

        public async Task<List<Hall>> LoadHalls()
        {
            _logger.Info($"Loading halls");
            string content = await Load(new Uri($"https://{App.UriBase}/api/Halls"));
            List<Hall> halls = JsonConvert.DeserializeObject<List<Hall>>(content);
            return halls;
        }

        public async Task<Stand> LoadStand(string hallId, string id)
        {
            _logger.Info($"Loading stand with id {id}");
            string content = await Load(new Uri($"https://{App.UriBase}/api/Stands/{hallId}/{id}"));
            Stand stand = JsonConvert.DeserializeObject<Stand>(content);
            return stand;
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
    }
}
