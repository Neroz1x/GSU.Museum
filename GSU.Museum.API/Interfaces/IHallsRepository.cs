using GSU.Museum.CommonClassLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Repository for model Hall
    /// </summary>
    public interface IHallsRepository
    {
        /// <summary>
        /// Return all halls
        /// </summary>
        /// <returns></returns>
        Task<List<Hall>> GetAllAsync();

        /// <summary>
        /// Return hall by id
        /// </summary>
        /// <param name="id">Id of the hall</param>
        /// <returns></returns>
        Task<Hall> GetAsync(string id);

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="entity">New record to add</param>
        /// <returns></returns>
        Task CreateAsync(Hall entity);

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <param name="id">Record id</param>
        /// <param name="entity">New record to update</param>
        /// <returns></returns>
        Task UpdateAsync(string id, Hall entity);

        /// <summary>
        /// Delete record from database
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <returns></returns>
        Task RemoveAsync(string id);

        /// <summary>
        /// Delete record from database
        /// </summary>
        /// <param name="entity">Entity to delete from database</param>
        /// <returns></returns>
        Task RemoveAsync(Hall entity);
    }
}
