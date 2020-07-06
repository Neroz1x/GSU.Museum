using GSU.Museum.API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Service for StandsController
    /// </summary>
    public interface IStandsService
    {
        /// <summary>
        /// Returns list of stands without nested exhibits
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="hallId">Hall id</param>
        /// <returns></returns>
        public Task<List<StandDTO>> GetAllAsync(HttpRequest request, string hallId);
        /// <summary>
        /// Retrieve stand by id with exhibits titles
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="hallId">Hall id</param>
        /// <param name="id">Stand id</param>
        /// <returns></returns>
        public Task<StandDTO> GetAsync(HttpRequest request, string hallId, string id);
    }
}
