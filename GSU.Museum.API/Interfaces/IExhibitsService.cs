using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Service 
    /// </summary>
    public interface IExhibitsService
    {
        /// <summary>
        /// Retrieve list of exhibits
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <returns></returns>
        public Task<List<ExhibitDTO>> GetAllAsync(HttpRequest request, string hallId, string standId);
        /// <summary>
        /// Retrieve exhibit by id
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="hallId">Id of hall</param>
        /// <param name="standId">Id of stand</param>
        /// <param name="id">Id of exhibit</param>
        /// <returns></returns>
        public Task<ExhibitDTO> GetAsync(HttpRequest request, string hallId, string standId, string id);
    }
}
