using GSU.Museum.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Interfaces
{
    /// <summary>
    /// Service for HomeController
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// Get all halls from database
        /// </summary>
        /// <returns>Return entities with localized titles and nested lists</returns>
        Task<List<HallViewModel>> GetAllAsync();
    }
}
