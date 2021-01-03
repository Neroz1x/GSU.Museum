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
    public class ExhibitsController : ControllerBase
    {
        private readonly IExhibitsRepository _exhibitsRepository;
        private readonly IExhibitsService _exhibitsService;
        private readonly ICompareService _compareService;

        public ExhibitsController(IExhibitsRepository exhibitRepository, IExhibitsService exhibitsService, ICompareService compareService)
        {
            _exhibitsRepository = exhibitRepository;
            _exhibitsService = exhibitsService;
            _compareService = compareService;
        }

        /// <summary>
        /// Returns all records
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Exhibits
        /// </remarks>
        /// <returns>All exhibits</returns>
        /// <response code="200">Everything is correct</response>
        /// <response code="404">Exhibits not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public IActionResult GetAll(string hallId, string standId)
        {
            var exhibits = _exhibitsService.GetAllAsync(Request, hallId, standId).Result;
            if(exhibits == null)
            {
                return NotFound();
            }
            return Ok(exhibits);
        }


        /// <summary>
        /// Gets specific record by id
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Exhibits/123456789012345678901234
        /// </remarks>
        /// <param name="id">Id of the record</param>
        /// <param name="hash">Hash from client</param>
        /// <returns>Record or not found</returns>
        /// GET: api/Exhibits/5
        /// <response code="200">Everything is correct. Hash codes are different</response>
        /// <response code="204">Everything is correct. Hash codes are the same</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("{hallId}/{standId}/{id}")]
        public async Task<IActionResult> GetAsync(string hallId, string standId, string id, int? hash)
        {
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }
            var exhibit = await _exhibitsService.GetAsync(Request, hallId, standId, id);
            if (exhibit == null)
            {
                return NotFound();
            }
            if (hash != null)
            {
                if (_compareService.IsEquals(hash.GetValueOrDefault(), exhibit))
                {
                    return NoContent();
                }
            }
            return Ok(exhibit);
        }

        /// <summary>
        /// Add record to database
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="exhibit">Record to add</param>
        /// <returns>Id of new record</returns>
        /// POST: api/Exhibits
        /// <response code="200">Everything is correct. Item has been created</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string hallId, string standId, Exhibit exhibit)
        {
            try
            {
                var id = await _exhibitsRepository.CreateAsync(hallId, standId, exhibit);
                
                // If hall or stand not found
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }
                return Ok(id);
            }
            catch (Exception)
            {
                throw new Error(Errors.Create_error, $"Can not create record {exhibit}");
            }
        }

        /// <summary>
        /// Updates specific record
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="exhibitIn">New record</param>
        /// <returns>Result of operation</returns>
        /// PUT: api/Exhibits/5
        /// <response code="204">Everything is correct</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(string hallId, string standId, Exhibit exhibitIn)
        {
            if (exhibitIn.Id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var exhibit = await _exhibitsRepository.GetAsync(hallId, standId, exhibitIn.Id);

            if (exhibit == null)
            {
                return NotFound();
            }

            try
            {
                await _exhibitsRepository.UpdateAsync(hallId, standId, exhibitIn.Id, exhibitIn);
            }
            catch (Exception)
            {
                throw new Error(Errors.Update_error, $"Can not update record {exhibit}");
            }
            return NoContent();
        }

        /// <summary>
        /// Delete specific record
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="id">Id of the record to delete</param>
        /// <returns>Result of the operation</returns>
        /// DELETE: api/Exhibits/5
        /// <response code="204">Everything is correct</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string hallId, string standId, string id)
        {
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var exhibit = await _exhibitsRepository.GetAsync(hallId, standId, id);

            if (exhibit == null)
            {
                return NotFound();
            }

            await _exhibitsRepository.RemoveAsync(hallId, standId, exhibit.Id);
            return NoContent();
        }
    }
}
