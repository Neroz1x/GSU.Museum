using GSU.Museum.CommonClassLibrary.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GSU.Museum.Shared.Interfaces
{
    /// <summary>
    /// Makes request and retrieve data
    /// </summary>
    interface IContentLoaderService
    {
        /// <summary>
        /// Send request to api/Halls
        /// </summary>
        /// <param name="cancelationToken">Cancellation token to cancel the request</param>
        /// <returns>List of halls without nested stands</returns>
        Task<List<HallDTO>> LoadHallsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Send request to api/Halls/ID
        /// </summary>
        /// <param name="id">Id of the hall</param>
        /// <param name="cancelationToken">Cancellation token to cancel the request</param>
        /// <returns>HallDTO and nested list of stands in this hall</returns>
        Task<HallDTO> LoadHallAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Send request to api/Stands/hallId/id
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="id">Id of the stand</param>
        /// <param name="cancelationToken">Cancellation token to cancel the request</param>
        /// <returns>StandDTO</returns>
        Task<StandDTO> LoadStandAsync(string hallId, string id, CancellationToken cancellationToken);

        /// <summary>
        /// Send request to api/Stands/hallId/standId/id
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="id">Id of the exhibit</param>
        /// <param name="cancelationToken">Cancellation token to cancel the request</param>
        /// <returns>ExhibitDTO</returns>
        Task<ExhibitDTO> LoadExhibitAsync(string hallId, string standId, string id, CancellationToken cancellationToken);
    }
}
