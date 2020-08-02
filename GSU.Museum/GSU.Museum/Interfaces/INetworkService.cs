using GSU.Museum.CommonClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        Task<List<HallDTO>> LoadHallsAsync();

        /// <summary>
        /// Load list of halls 
        /// </summary>
        /// <param name="hallsCached">Halls from cache to compare hashes</param>
        /// <returns>List of halls or hallsCached if hash codes are equal</returns>
        Task<List<HallDTO>> LoadHallsAsync(List<HallDTO> hallsCached);

        /// <summary>
        /// Load hall by id
        /// </summary>
        /// <param name="id">Id of hall</param>
        /// <returns>HallDTO</returns>
        Task<HallDTO> LoadHallAsync(string id);

        /// <summary>
        /// Load hall by id
        /// </summary>
        /// <param name="id">Id of hall</param>
        /// <param name="hallCached">HallDTO from cache to compare hashes</param>
        /// <returns>HallDTO or hallCached if hash codes are equal</returns>
        Task<HallDTO> LoadHallAsync(string id, HallDTO hallCached);

        /// <summary>
        /// Load stand by hall id and stand id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="id">Id of stand</param>
        /// <returns>StandDTO</returns>
        Task<StandDTO> LoadStandAsync(string hallId, string id);

        /// <summary>
        /// Load stand by hall id and stand id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="id">Id of stand</param>
        /// <param name="standCached">StandDTO from cache to compare hashes</param>
        /// <returns>StandDTO or standCached if hash codes are equal</returns>
        Task<StandDTO> LoadStandAsync(string hallId, string id, StandDTO standCached);

        /// <summary>
        /// Load exhibit by hall id, stand id and exhibit id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Id of exhibit</param>
        /// <returns>ExhibitDTO</returns>
        Task<ExhibitDTO> LoadExhibitAsync(string hallId, string standId, string id);

        /// <summary>
        /// Load exhibit by hall id, stand id and exhibit id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Id of exhibit</param>
        /// <param name="exhibitCached">ExhibitDTO from cache to compare hashes</param>
        /// <returns>ExhibitDTO or exhibitCached if hash codes are equal</returns>
        Task<ExhibitDTO> LoadExhibitAsync(string hallId, string standId, string id, ExhibitDTO exhibitCached);

        /// <summary>
        /// Load cache
        /// </summary>
        /// <returns></returns>
        Task LoadCacheAsync();

        /// <summary>
        /// Perform request and return content as Stream
        /// </summary>
        /// <param name="uri">Uri to perform request</param>
        /// <returns></returns>
        Task<Stream> LoadStreamAsync(Uri uri);
    }
}
