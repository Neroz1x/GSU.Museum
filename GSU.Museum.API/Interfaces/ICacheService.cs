using Akavache.Sqlite3;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
        Task WriteHallAsync(HallDTO hall);

        /// <summary>
        /// Write list of halls
        /// </summary>
        /// <param name="halls">Hals to cache</param>
        Task WriteHallsAsync(List<HallDTO> halls);

        /// <summary>
        /// Write stand to cache
        /// </summary>
        /// <param name="stand">StandDTO to cache</param>
        Task WriteStandAsync(StandDTO stand);

        /// <summary>
        /// Write exhibit to cache
        /// </summary>
        /// <param name="exhibit">ExhibitDTO to cache</param>
        Task WriteExhibitAsync(ExhibitDTO exhibit);

        /// <summary>
        /// Create cache with specific language
        /// </summary>
        /// <param name="httpRequest">Request with Accept-Language header</param>
        /// <returns></returns>
        Task CreateCache(HttpRequest httpRequest);

        /// <summary>
        /// Return db file
        /// </summary>
        /// <param name="httpRequest">Request with Accept-Language header</param>
        /// <returns></returns>
        public FileStream GetCahceDB(HttpRequest httpRequest);

        /// <summary>
        /// Retrun db-shm file
        /// </summary>
        /// <param name="httpRequest">Request with Accept-Language header</param>
        /// <returns></returns>
        public FileStream GetCahceDBSHM(HttpRequest httpRequest);

        /// <summary>
        /// Retrun db-wal file
        /// </summary>
        /// <param name="httpRequest">Request with Accept-Language header</param>
        /// <returns></returns>
        public FileStream GetCahceDBWAL(HttpRequest httpRequest);

        /// <summary>
        /// Check cache version
        /// </summary>
        /// <param name="version">Version of client's cache</param>
        /// <param name="httpRequest">Http request with language header</param>
        /// <returns>True if equals; otherwise - false</returns>
        public Task<bool> IsUpToDate(int version, HttpRequest httpRequest);
    }
}
