using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Repository class
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Return all records
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Return record by id
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <returns></returns>
        Task<T> GetAsync(string id);

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="entity">New record to add</param>
        /// <returns></returns>
        Task CreateAsync(T entity);

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <param name="id">Record id</param>
        /// <param name="entity">New record to update</param>
        /// <returns></returns>
        Task UpdateAsync(string id, T entity);

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
        Task RemoveAsync(T entity);
    }
}
