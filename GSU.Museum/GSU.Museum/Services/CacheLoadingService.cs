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
        public async Task LoadLanguageCacheAsync(string language, string status)
        {
            var cachingService = DependencyService.Get<CachingService>();

            var versionKey = $"v_{language}";
            var version = await cachingService.ReadCache(versionKey);
            
            var url = new Uri($"{App.UriBase}/api/Cache/{language}{ (version == 0 ? string.Empty : $"?version={version}")}");
            var stream = await DependencyService.Get<NetworkService>().LoadStreamAsync(url);

            if (stream is null)
            {
                return;
            }

            await cachingService.ClearCache($"v_{language}");
            await cachingService.ClearCache(language);
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string versionString = await reader.ReadLineAsync();
                await cachingService.WriteCache(versionKey, GetVersion(versionString));

                string data = await reader.ReadToEndAsync();
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                foreach (var item in dictionary)
                {
                    await cachingService.WriteCache(item.Key, item.Value);
                }
            }
        }

        public async Task LoadPhotosCacheAsync(string status)
        {
            var cachingService = DependencyService.Get<CachingService>();

            var versionKey = $"v_photos";
            var version = await cachingService.ReadCache(versionKey);

            var url = new Uri($"{App.UriBase}/api/Cache{ (version == 0 ? string.Empty : $"?version={version}")}");
            var stream = await DependencyService.Get<NetworkService>().LoadStreamAsync(url);
            
            if(stream is null)
            {
                return;
            }

            await cachingService.ClearCache($"v_photo");
            await cachingService.ClearCache("photo");

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {                
                string versionString = await reader.ReadLineAsync();
                await cachingService.WriteCache(versionKey, GetVersion(versionString));
                
                string data = await reader.ReadToEndAsync();
                
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                foreach(var item in dictionary)
                {
                    await cachingService.WriteCache(item.Key, item.Value);
                }
            }
        }

        private uint GetVersion(string versionString)
        {
            var versionFromFileString = versionString.Substring(versionString.IndexOf(':') + 1) ?? string.Empty;
            uint.TryParse(versionFromFileString, out uint currentVersion);
            return currentVersion;
        }
    }
}
