using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSU.Museum.API.Data.Enums;
using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitsController : ControllerBase
    {
        private readonly IExhibitsRepository _exhibitsRepository;
        private readonly IExhibitsService _exhibitsService;

        public ExhibitsController(IExhibitsRepository exhibitRepository, IExhibitsService exhibitsService)
        {
            _exhibitsRepository = exhibitRepository;
            _exhibitsService = exhibitsService;
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
            Request.Headers.TryGetValue("Language", out var recievedLanguage);
            return await _exhibitsService.GetAllAsync(recievedLanguage, hallId, standId);
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
        /// <returns>Record or not found</returns>
        // GET: api/Exhibits/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string hallId, string standId, string id)
        {
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var stand = await _exhibitsRepository.GetAsync(hallId, standId, id);
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
        /// <param name="standId">Id of the stand</param>
        /// <param name="stand">Record to add</param>
        /// <returns>Result of the operation</returns>
        // POST: api/Exhibits
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string hallId, string standId, Exhibit stand)
        {
            try
            {
                await _exhibitsRepository.CreateAsync(hallId, standId, stand);
                return Ok();
            }
            catch (Exception)
            {
                throw new Exception();
                throw new Error(Errors.Create_error, $"Can not add record {stand}");
            }
        }

        /// <summary>
        /// Updates specific reord
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

            var stand = await _exhibitsRepository.GetAsync(hallId, standId, exhibitIn.Id);

            if (stand == null)
            {
                return NotFound();
            }

            try
            {
                await _exhibitsRepository.UpdateAsync(hallId, standId, exhibitIn.Id, exhibitIn);
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

            var stand = await _exhibitsRepository.GetAsync(hallId, standId, id);

            if (stand == null)
            {
                return NotFound();
            }

            await _exhibitsRepository.RemoveAsync(hallId, standId, stand.Id);
            return NoContent();
        }
    }
}
