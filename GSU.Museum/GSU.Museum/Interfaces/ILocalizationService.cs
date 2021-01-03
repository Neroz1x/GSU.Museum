using System;
using System.Collections.Generic;
using System.Text;

namespace GSU.Museum.Shared.Interfaces
{
    /// <summary>
    /// Service helps localize app
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Change UI culture by Setttings' language
        /// </summary>
        void Localize();
    }
}
