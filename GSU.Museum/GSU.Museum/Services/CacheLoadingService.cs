using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CacheLoadingService))]
namespace GSU.Museum.Shared.Services
{
    public class CacheLoadingService : ICacheLoadingService
    {
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<Uri> GetUrlAsync(string language)
        {
            var cachingService = DependencyService.Get<CachingService>();

            var versionKey = $"v_{language}";
            var version = await cachingService.ReadCache(versionKey);
            
            return new Uri($"{App.UriBase}/api/Cache/{language}{ (version == 0 ? string.Empty : $"?version={version}")}");
        }

        public async Task<Uri> GetUrlAsync()
        {
            var cachingService = DependencyService.Get<CachingService>();

            var versionKey = $"v_photos";
            var version = await cachingService.ReadCache(versionKey);

            return new Uri($"{App.UriBase}/api/Cache{ (version == 0 ? string.Empty : $"?version={version}")}");  
        }

        public async Task WriteCacheAsync(Stream stream, string versionKey, string keyAlias)
        {
            if(stream is null || stream?.Length == 0)
            {
                return;
            }
            _logger.Info($"Removing previous version: {versionKey}");

            var cachingService = DependencyService.Get<CachingService>();

            await cachingService.ClearCache(versionKey);
            await cachingService.ClearCache(keyAlias);

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string versionString = await reader.ReadLineAsync();
                var version = GetVersion(versionString);
                await cachingService.WriteCache(versionKey, version);

                _logger.Info($"Writting new version version: {version} with {versionKey}");

                string data = await reader.ReadToEndAsync();

                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                foreach (var item in dictionary)
                {
                    await cachingService.WriteCache(item.Key, item.Value);
                }
            }
        }

        private uint GetVersion(string versionString)
        {
            uint currentVersion = 0;
            var versionFromFileString = versionString.Substring(versionString.IndexOf(':') + 1) ?? string.Empty;
            uint.TryParse(versionFromFileString, out currentVersion);
            return currentVersion;
        }
    }
}
