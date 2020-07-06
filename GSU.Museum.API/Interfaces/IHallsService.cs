using GSU.Museum.API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Interfaces
{
    /// <summary>
    /// Service to perform get hall request
    /// </summary>
    public interface IHallsService
    {
        /// <summary>
        /// Retrieve all halls whithout nested list of stands
        /// </summary>
        /// <param name="request">Http request</param>
        /// <returns></returns>
        public Task<List<HallDTO>> GetAllAsync(HttpRequest request);

        /// <summary>
        /// Retrieve hall by id with stands titles
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="id">Id of the hall</param>
        /// <returns></returns>
        public Task<HallDTO> GetAsync(HttpRequest request, string id);
    }
}
