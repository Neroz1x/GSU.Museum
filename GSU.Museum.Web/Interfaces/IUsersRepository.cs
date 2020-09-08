using GSU.Museum.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Interfaces
{
    /// <summary>
    /// Repository for model User
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetAllAsync();

        /// <summary>
        /// Return user by login
        /// </summary>
        /// <param name="login">Login of the user</param>
        /// <returns></returns>
        Task<User> GetAsync(string login);
        
        /// <summary>
        /// Return user by id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns></returns>
        Task<User> GetByIdAsync(string id);

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user">New user to add</param>
        /// <returns>An id of new user</returns>
        Task<string> CreateAsync(User user);

        /// <summary>
        /// Update existing user
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="user">New record to update</param>
        /// <returns></returns>
        Task UpdateAsync(string id, User user);

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns></returns>
        Task RemoveAsync(string id);
    }
}
