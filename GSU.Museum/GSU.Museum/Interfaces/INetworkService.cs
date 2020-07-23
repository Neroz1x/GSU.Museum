using GSU.Museum.Shared.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSU.Museum.Shared.Interfaces
{
    /// <summary>
    /// Service performs requests to the API
    /// </summary>
    public interface INetworkService
    {
        /// <summary>
        /// Performs request to the API
        /// </summary>
        /// <param name="uri">Uri of request</param>
        /// <returns>Content of the response</returns>
        Task<string> LoadAsync(Uri uri);

        /// <summary>
        /// Create HttpClient with essential headders
        /// </summary>
        /// <returns>HttpClient</returns>
        HttpClient GetHttpClient();

        /// <summary>
        /// Load list of halls
        /// </summary>
        /// <returns>List of halls</returns>
        Task<List<Hall>> LoadHallsAsync();

        /// <summary>
        /// Load list of halls 
        /// </summary>
        /// <param name="hallsCached">Halls from cache to compare hashes</param>
        /// <returns>List of halls or hallsCached if hash codes are equal</returns>
        Task<List<Hall>> LoadHallsAsync(List<Hall> hallsCached);

        /// <summary>
        /// Load hall by id
        /// </summary>
        /// <param name="id">Id of hall</param>
        /// <returns>Hall</returns>
        Task<Hall> LoadHallAsync(string id);

        /// <summary>
        /// Load hall by id
        /// </summary>
        /// <param name="id">Id of hall</param>
        /// <param name="hallCached">Hall from cache to compare hashes</param>
        /// <returns>Hall or hallCached if hash codes are equal</returns>
        Task<Hall> LoadHallAsync(string id, Hall hallCached);

        /// <summary>
        /// Load stand by hall id and stand id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="id">Id of stand</param>
        /// <returns>Stand</returns>
        Task<Stand> LoadStandAsync(string hallId, string id);

        /// <summary>
        /// Load stand by hall id and stand id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="id">Id of stand</param>
        /// <param name="standCached">Stand from cache to compare hashes</param>
        /// <returns>Stand or standCached if hash codes are equal</returns>
        Task<Stand> LoadStandAsync(string hallId, string id, Stand standCached);

        /// <summary>
        /// Load exhibit by hall id, stand id and exhibit id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Id of exhibit</param>
        /// <returns>Exhibit</returns>
        Task<Exhibit> LoadExhibitAsync(string hallId, string standId, string id);

        /// <summary>
        /// Load exhibit by hall id, stand id and exhibit id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Id of exhibit</param>
        /// <param name="exhibitCached">Exhibit from cache to compare hashes</param>
        /// <returns>Exhibit or exhibitCached if hash codes are equal</returns>
        Task<Exhibit> LoadExhibitAsync(string hallId, string standId, string id, Exhibit exhibitCached);
    }
}
