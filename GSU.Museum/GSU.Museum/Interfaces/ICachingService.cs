using GSU.Museum.Shared.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        Task<List<Hall>> ReadHallsAsync();

        /// <summary>
        /// Check if hall exists in chache and retrieve it in current language
        /// </summary>
        /// <param name="id">Id of the hall</param>
        /// <returns>Hall if exists; else - null</returns>
        Task<Hall> ReadHallAsync(string id);

        /// <summary>
        /// Check if stand exists in chache and retrieve it in current language
        /// </summary>
        /// <param name="id">Id of the stand</param>
        /// <returns>Stand if exists; else - null</returns>
        Task<Stand> ReadStandAsync(string id);

        /// <summary>
        /// Check if exhibit exists in chache and retrieve it in current language
        /// </summary>
        /// <param name="id">Id of the exhibit</param>
        /// <returns>Exhibit if exists; else - null</returns>
        Task<Exhibit> ReadExhibitAsync(string id);

        /// <summary>
        /// Write list of halls
        /// </summary>
        /// <param name="halls">Hals to cache</param>
        Task WriteHallsAsync(List<Hall> halls);

        /// <summary>
        /// Write hall to cache
        /// </summary>
        /// <param name="hall">Hall to cache</param>
        Task WriteHallAsync(Hall hall);

        /// <summary>
        /// Write stand to cache
        /// </summary>
        /// <param name="stand">Stand to cache</param>
        Task WriteStandAsync(Stand stand);

        /// <summary>
        /// Write exhibit to cache
        /// </summary>
        /// <param name="exhibit">Exhibit to cache</param>
        Task WriteExhibitAsync(Exhibit exhibit);
    }
}
