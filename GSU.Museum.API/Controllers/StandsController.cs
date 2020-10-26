using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSU.Museum.API.Filters;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.API.Controllers
{
    [ApiKeyAuth]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StandsController : ControllerBase
    {
        private readonly IStandsRepository _standsRepository;
        private readonly IStandsService _standsService;
        private readonly ICompareService _compareService;

        public StandsController(IStandsRepository exhibitRepository, IStandsService standsService, ICompareService compareService)
        {
            _standsRepository = exhibitRepository;
            _standsService = standsService;
            _compareService = compareService;
        }

        /// <summary>
        /// Returns all records
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Stands
        /// </remarks>
        /// <returns>All stands without nested exhibits</returns>
        /// <response code="200">Everything is correct</response>
        /// <response code="404">Stands not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetAll(string hallId)
        {
            var stands = _standsService.GetAllAsync(Request, hallId).Result;
            if(stands == null)
            {
                return NotFound();
            }
            return Ok(stands);
        }


        /// <summary>
        /// Gets specific record by id
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///     GET: api/Stands/123456789012345678901234
        /// </remarks>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="id">Id of the record</param>
        /// <param name="hash">Hash from client</param>
        /// <returns>Record or not found</returns>
        /// GET: api/Stands/5
        /// <response code="200">Everything is correct. Hash codes are different</response>
        /// <response code="204">Everything is correct. Hash codes are the same</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("{hallId}/{id}")]
        public async Task<IActionResult> GetAsync(string hallId, string id, int? hash)
        {
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var stand = await _standsService.GetAsync(Request, hallId, id);
            if (stand == null)
            {
                return NotFound();
            }
            if (hash != null)
            {
                if (_compareService.IsEquals(hash.GetValueOrDefault(), stand))
                {
                    return NoContent();
                }
            }
            return Ok(stand);
        }

        /// <summary>
        /// Add record to database
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="stand">Record to add</param>
        /// <returns>Id of new record</returns>
        /// POST: api/Stands
        /// <response code="200">Everything is correct. Item has been created</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string hallId, Stand stand)
        {
            try
            {
                var id = await _standsRepository.CreateAsync(hallId, stand);
                
                // If hall not found
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }
                return Ok(id);
            }
            catch (Exception)
            {
                throw new Error(Errors.Create_error, $"Can not add record {stand}");
            }
        }

        /// <summary>
        /// Updates specific record
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standIn">New record</param>
        /// <returns>Result of operation</returns>
        /// PUT: api/Stands/5
        /// <response code="204">Everything is correct</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(string hallId, Stand standIn)
        {
            if (standIn.Id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var stand = await _standsRepository.GetAsync(hallId, standIn.Id);

            if (stand == null)
            {
                return NotFound();
            }

            try
            {
                await _standsRepository.UpdateAsync(hallId, standIn.Id, standIn);
            }
            catch (Exception)
            {
                throw new Error(Errors.Update_error, $"Can not update record {stand}");
            }
            return NoContent();
        }

        /// <summary>
        /// Delete specific record
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="id">Id of the record to delete</param>
        /// <returns>Result of the operation</returns>
        /// DELETE: api/Stands/5
        /// <response code="204">Everything is correct</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string hallId, string id)
        {
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var stand = await _standsRepository.GetAsync(hallId, id);

            if (stand == null)
            {
                return NotFound();
            }

            await _standsRepository.RemoveAsync(hallId, stand.Id);
            return NoContent();
        }
    }
}
