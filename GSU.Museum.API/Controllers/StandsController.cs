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
    public class StandsController : ControllerBase
    {
        private readonly IStandsRepository _standsRepository;
        private readonly IStandsService _standsService;

        public StandsController(IStandsRepository exhibitRepository, IStandsService standsService)
        {
            _standsRepository = exhibitRepository;
            _standsService = standsService;
        }

        /// <summary>
        /// Returns all records
        /// </summary>
        /// <param name="hallId">Id f the hall</param>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Stands
        /// </remarks>
        /// <returns>All stands without nested exhibits</returns>
        [HttpGet]
        public async Task<List<StandDTO>> GetAll(string hallId)
        {   if(!Request.Headers.TryGetValue("Language", out var recievedLanguage))
            {
                recievedLanguage = "En";
            }
            return await _standsService.GetAllAsync(recievedLanguage, hallId);
        }


        /// <summary>
        /// Gets specific record by id
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///     GET: api/Exhibits/123456789012345678901234
        /// </remarks>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="id">Id of the record</param>
        /// <returns>Record or not found</returns>
        // GET: api/Stands/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string hallId, string id)
        {
            if (!Request.Headers.TryGetValue("Language", out var recievedLanguage))
            {
                recievedLanguage = "En";
            }
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var stand = await _standsService.GetAsync(recievedLanguage, hallId, id);
            if (stand == null)
            {
                return NotFound();
            }
            return Ok(stand);
        }

        /// <summary>
        /// Add record to database
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="stand">Record to add</param>
        /// <returns>Result of the operation</returns>
        // POST: api/Stands
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string hallId, Stand stand)
        {
            try
            {
                await _standsRepository.CreateAsync(hallId, stand);
                return Ok();
            }
            catch (Exception)
            {
                throw new Error(Errors.Create_error, $"Can not add record {stand}");
            }
        }

        /// <summary>
        /// Updates specific reord
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standIn">New record</param>
        /// <returns>Result of operation</returns>
        // PUT: api/Stands/5
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
        // DELETE: api/Stands/5
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
