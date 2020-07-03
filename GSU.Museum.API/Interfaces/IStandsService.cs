using GSU.Museum.API.Data.Models;
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
        /// <param name="language">Language from header</param>
        /// <param name="hallId">Hall id</param>
        /// <returns></returns>
        public Task<List<StandDTO>> GetAllAsync(StringValues language, string hallId);
        /// <summary>
        /// Retrieve stand by id with exhibits titles
        /// </summary>
        /// <param name="language">Language from header</param>
        /// <param name="hallId">Hall id</param>
        /// <param name="id">Stand id</param>
        /// <returns></returns>
        public Task<StandDTO> GetAsync(StringValues language, string hallId, string id);
    }
}
