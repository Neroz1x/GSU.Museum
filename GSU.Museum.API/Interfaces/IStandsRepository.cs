using GSU.Museum.API.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Repository for entity Stand
    /// </summary>
    public interface IStandsRepository
    {
 
        /// <summary>
        /// Returns list of stands in hall
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <returns></returns>
        Task<List<Stand>> GetAllAsync(string hallId);

        /// <summary>
        /// Return stand by id
        /// </summary>
        /// <param name="hallId">Hall id</param>
        /// <param name="id">Stand id</param>
        /// <returns></returns>
        Task<Stand> GetAsync(string hallId, string id);

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="hallId">Hall id</param>
        /// <param name="entity">New record to add</param>
        /// <returns></returns>
        Task CreateAsync(string hallId, Stand entity);

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <param name="hallId">Hall id</param>
        /// <param name="id">Record id</param>
        /// <param name="entity">New record to update</param>
        /// <returns></returns>
        Task UpdateAsync(string hallId, string id, Stand entity);

        /// <summary>
        /// Delete record from database
        /// </summary>
        /// <param name="hallId">Hall id</param>
        /// <param name="id">Id of the record</param>
        /// <returns></returns>
        Task RemoveAsync(string hallId, string id);

    }
}
