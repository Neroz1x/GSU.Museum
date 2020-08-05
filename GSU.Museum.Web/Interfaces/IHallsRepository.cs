using GSU.Museum.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Interfaces
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
        Task<List<HallViewModel>> GetAllAsync();

        /// <summary>
        /// Return hall by id
        /// </summary>
        /// <param name="id">Id of the hall</param>
        /// <returns></returns>
        Task<HallViewModel> GetAsync(string id);

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="entity">New record to add</param>
        /// <returns>An id of new record</returns>
        Task<string> CreateAsync(HallViewModel entity);

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <param name="id">Record id</param>
        /// <param name="entity">New record to update</param>
        /// <returns></returns>
        Task UpdateAsync(string id, HallViewModel entity);

        /// <summary>
        /// Delete record from database
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <returns></returns>
        Task RemoveAsync(string id);
    }
}
