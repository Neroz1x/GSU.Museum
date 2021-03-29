using GSU.Museum.CommonClassLibrary.Interfaces;
using GSU.Museum.CommonClassLibrary.Models;
using System.Collections.Generic;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Service to compare object from database and mobile cache by HashCode
    /// </summary>
    public interface ICompareService
    {
        /// <summary>
        /// Compares item from database and mobile cache
        /// </summary>
        /// <param name="hashCode">HashCode from mobile</param>
        /// <param name="item">Item from database</param>
        /// <returns>True if equals; else - false</returns>
        public bool IsEquals(int hashCode, IMuseumItemDTO item);

        /// <summary>
        /// Compares list of halls from database and mobile cache
        /// </summary>
        /// <param name="hashCode">HashCode from mobile</param>
        /// <param name="halls">List of halls from database</param>
        /// <returns>True if equals; else - false</returns>
        public bool IsListEquals(int hashCode, List<HallDTO> halls);
    }
}
