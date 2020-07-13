using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSU.Museum.API.Data.Enums;
using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Filters;
using GSU.Museum.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.API.Controllers
{
    [ApiKeyAuth]
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
        [HttpGet]
        public async Task<List<ExhibitDTO>> GetAll(string hallId, string standId)
        {
            return await _exhibitsService.GetAllAsync(Request, hallId, standId);
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
        // GET: api/Exhibits/5
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
        /// <returns>Result of the operation</returns>
        // POST: api/Exhibits
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string hallId, string standId, Exhibit exhibit)
        {
            try
            {
                await _exhibitsRepository.CreateAsync(hallId, standId, exhibit);
                return Ok();
            }
            catch (Exception)
            {
                throw new Error(Errors.Create_error, $"Can not add record {exhibit}");
            }
        }

        /// <summary>
        /// Updates specific record
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="exhibitIn">New record</param>
        /// <returns>Result of operation</returns>
        // PUT: api/Exhibits/5
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
        // DELETE: api/Exhibits/5
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
