using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Service for CacheController
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Write hall to cache
        /// </summary>
        /// <param name="hall">HallDTO to cache</param>
        /// <param name="cache"></param>
        /// <param name="cachePhotos"></param>
        /// <param name="language"></param>
        void WriteHallAsync(HallDTO hall, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language);

        /// <summary>
        /// Write list of halls
        /// </summary>
        /// <param name="halls">Hals to cache</param>
        /// <param name="cache"></param>
        /// <param name="cachePhotos"></param>
        /// <param name="language"></param>
        void WriteHallsAsync(List<HallDTO> halls, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language);

        /// <summary>
        /// Write stand to cache
        /// </summary>
        /// <param name="stand">StandDTO to cache</param>
        /// <param name="cache"></param>
        /// <param name="cachePhotos"></param>
        /// <param name="language"></param>
        void WriteStandAsync(StandDTO stand, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language);

        /// <summary>
        /// Write exhibit to cache
        /// </summary>
        /// <param name="exhibit">ExhibitDTO to cache</param>
        /// <param name="cache"></param>
        /// <param name="cachePhotos"></param>
        /// <param name="language"></param>
        void WriteExhibitAsync(ExhibitDTO exhibit, Dictionary<string, object> cache, Dictionary<string, object> cachePhotos, string language);

        /// <summary>
        /// Create cache with specific language
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="languageList">List of languages to create cache</param>
        /// <param name="savePhotos">Is photos have to be cached</param>
        /// <returns></returns>
        Task CreateCache(HttpRequest httpRequest, IEnumerable<string> languageList, bool savePhotos = true);

        /// <summary>
        /// Get cached text of specified text
        /// </summary>
        /// <param name="language">Language of cache</param>
        /// <param name="version">Version of previous cache</param>
        /// <returns></returns>
        Stream GetCache(string language, uint version = 0);

        /// <summary>
        /// Get cached photos
        /// </summary>
        /// <param name="version">Version of previous cache</param>
        /// <returns></returns>
        public Stream GetCache(uint version = 0);

        /// <summary>
        /// Get cache version
        /// </summary>
        /// <param name="path">Path to cache</param>
        /// <returns></returns>
        public uint GetVersion(string path);
    }
}
