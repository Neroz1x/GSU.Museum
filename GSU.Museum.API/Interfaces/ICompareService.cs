﻿using GSU.Museum.API.Data.Models;
using System.Collections.Generic;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Service to compare object from database and mobile cache by HashCode
    /// </summary>
    public interface ICompareService
    {
        /// <summary>
        /// Compares hall from database and mobile cache
        /// </summary>
        /// <param name="hashCode">HashCode from mobile</param>
        /// <param name="hall">Hall from database</param>
        /// <returns>True if equals; else - false</returns>
        public bool IsEquals(int hashCode, HallDTO hall);

        /// <summary>
        /// Compares stand from database and mobile cache
        /// </summary>
        /// <param name="hashCode">HashCode from mobile</param>
        /// <param name="stand">Stand from database</param>
        /// <returns>True if equals; else - false</returns>
        public bool IsEquals(int hashCode, StandDTO stand);

        /// <summary>
        /// Compares exhibit from database and mobile cache
        /// </summary>
        /// <param name="hashCode">HashCode from mobile</param>
        /// <param name="exhibit">Exhibit from database</param>
        /// <returns>True if equals; else - false</returns>
        public bool IsEquals(int hashCode, ExhibitDTO exhibit);

        /// <summary>
        /// Compares list of halls from database and mobile cache
        /// </summary>
        /// <param name="hashCode">HashCode from mobile</param>
        /// <param name="halls">List of halls from database</param>
        /// <returns>True if equals; else - false</returns>
        public bool IsListEquals(int hashCode, List<HallDTO> halls);
    }
}