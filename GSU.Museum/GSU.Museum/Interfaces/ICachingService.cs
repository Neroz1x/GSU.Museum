using System.Threading.Tasks;
using System.Collections.Generic;
using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Data.Models;

namespace GSU.Museum.Shared.Interfaces
{
    /// <summary>
    /// Service helps to save and retrieve data
    /// </summary>
    public interface ICachingService
    {
        /// <summary>
        /// Check if halls exhist in cache and retrieve them from it in current language
        /// </summary>
        /// <param name="halls">Halls from cahce</param>
        /// <returns>List of halls if exists; else - null</returns>
        Task<List<HallDTO>> ReadHallsAsync();

        /// <summary>
        /// Check if hall exists in chache and retrieve it in current language
        /// </summary>
        /// <param name="id">Id of the hall</param>
        /// <returns>HallDTO if exists; else - null</returns>
        Task<HallDTO> ReadHallAsync(string id);

        /// <summary>
        /// Check if stand exists in chache and retrieve it in current language
        /// </summary>
        /// <param name="id">Id of the stand</param>
        /// <returns>StandDTO if exists; else - null</returns>
        Task<StandDTO> ReadStandAsync(string id);

        /// <summary>
        /// Check if exhibit exists in chache and retrieve it in current language
        /// </summary>
        /// <param name="id">Id of the exhibit</param>
        /// <returns>ExhibitDTO if exists; else - null</returns>
        Task<ExhibitDTO> ReadExhibitAsync(string id);

        /// <summary>
        /// Read user settings from cache
        /// </summary>
        /// <returns>If null - returns default settings</returns>
        Task<Settings> ReadSettings();
        /// <summary>
        /// Write list of halls
        /// </summary>
        /// <param name="halls">Halls to cache</param>
        Task WriteHallsAsync(List<HallDTO> halls);

        /// <summary>
        /// Write hall to cache
        /// </summary>
        /// <param name="hall">HallDTO to cache</param>
        Task WriteHallAsync(HallDTO hall);

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
        /// Write user settings to cahce
        /// </summary>
        /// <returns></returns>
        Task WriteSettings();

        /// <summary>
        /// Write key value pairs
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task WriteCache(string key, object value);
        
        /// <summary>
        /// Read cache
        /// </summary>
        /// <param name="key"></param>
        Task<uint> ReadCache(string key);

        /// <summary>
        /// Remove all cache
        /// </summary>
        /// <returns></returns>
        Task ClearCache();

        /// <summary>
        /// Clear data from cache which keys contains specific substring
        /// </summary>
        /// <returns></returns>
        Task ClearCache(string substring);
    }
}
