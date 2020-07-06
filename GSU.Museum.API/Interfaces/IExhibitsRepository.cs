using GSU.Museum.API.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Repository for model Exhibit
    /// </summary>
    public interface IExhibitsRepository
    {
        /// <summary>
        /// Returns all exhibits
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <returns></returns>
        Task<List<Exhibit>> GetAllAsync(string hallId, string standId);

        /// <summary>
        /// Return exhibit by id
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Id of exhibit</param>
        /// <returns></returns>
        Task<Exhibit> GetAsync(string hallId, string standId, string id);

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="entity">New record to add</param>
        /// <returns></returns>
        Task CreateAsync(string hallId, string standId, Exhibit entity);

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Record id</param>
        /// <param name="entity">New record to update</param>
        /// <returns></returns>
        Task UpdateAsync(string hallId, string standId, string id, Exhibit entity);

        /// <summary>
        /// Delete record from database
        /// </summary>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Id of the record</param>
        /// <returns></returns>
        Task RemoveAsync(string hallId, string standId, string id);
    }
}
