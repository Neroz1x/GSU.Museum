using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Interfaces
{
    /// <summary>
    /// Convert IFormFile to bytes
    /// </summary>
    public interface IFormFileToByteConverterService
    {
        /// <summary>
        /// Convert IFormFile to bytes
        /// </summary>
        /// <param name="file">IFormFile instance</param>
        /// <param name="stand">Object with byte array</param>
        /// <returns></returns>
        Task ConvertAsync(IFormFile file, StandViewModel stand);

        /// <summary>
        /// Convert IFormFile to bytes
        /// </summary>
        /// <param name="file">IFormFile instance</param>
        /// <param name="hall">Object with byte array</param>
        /// <returns></returns>
        Task ConvertAsync(IFormFile file, HallViewModel hall);

        /// <summary>
        /// Convert IFormFile to bytes
        /// </summary>
        /// <param name="files">IFormFile instance</param>
        /// <param name="exhibit">Object with list of byte arrays</param>
        /// <returns></returns>
        Task ConvertAsync(IEnumerable<IFormFile> files, ExhibitViewModel exhibit);
    }
}
