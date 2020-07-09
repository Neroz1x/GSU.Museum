using GSU.Museum.Shared.Data.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSU.Museum.Shared.Interfaces
{
    /// <summary>
    /// Makes request and retrieve data
    /// </summary>
    interface IContentLoaderService
    {
        /// <summary>
        /// Send request to api/Halls
        /// </summary>
        /// <returns>List of halls without nested stands</returns>
        Task<List<Hall>> LoadHalls();

        /// <summary>
        /// Send request to api/Halls/ID
        /// </summary>
        /// <param name="id">Id of the hall</param>
        /// <returns>Hall and nested list of stands in this hall</returns>
        Task<Hall> LoadHall(string id);

        /// <summary>
        /// Send request to api/Stands/hallId/id
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="id">Id of the stand</param>
        /// <returns>Stand</returns>
        Task<Stand> LoadStand(string hallId, string id);

        /// <summary>
        /// Send request to api/Stands/hallId/standId/id
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="id">Id of the exhibit</param>
        /// <returns>Exhibit</returns>
        Task<Exhibit> LoadExhibit(string hallId, string standId, string id);

        /// <summary>
        /// Create HttpClient with ApiKey
        /// </summary>
        /// <returns>HttpClient with sertificate</returns>
        HttpClient GetHttpClient();
    }
}
