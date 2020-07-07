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
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly IHallsRepository _hallsRepository;
        private readonly IHallsService _hallsService;

        public HallsController(IHallsRepository exhibitRepository, IHallsService hallsService)
        {
            _hallsRepository = exhibitRepository;
            _hallsService = hallsService;
        }

        /// <summary>
        /// Returns all records
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Halls
        /// </remarks>
        /// <returns>Halls without nested stands</returns>
        [HttpGet]
        public async Task<List<HallDTO>> GetAll()
        {
            return await _hallsService.GetAllAsync(Request);
        }


        /// <summary>
        /// Gets specific record by id
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///     GET: api/Halls/123456789012345678901234
        /// </remarks>
        /// <param name="id">Id of the record</param>
        /// <returns>Record or not found</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var hall = await _hallsService.GetAsync(Request, id);
            if (hall == null)
            {
                return NotFound();
            }
            return Ok(hall);
        }

        /// <summary>
        /// Add record to database
        /// </summary>
        /// <param name="hall">Record to add</param>
        /// <returns>Result of the operation</returns>
        // POST: api/Halla
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Hall hall)
        {
            try
            {
                await _hallsRepository.CreateAsync(hall);
                return Ok();
            }
            catch (Exception)
            {
                throw new Exception();
                throw new Error(Errors.Create_error, $"Can not add record {hall}");
            }
        }

        /// <summary>
        /// Updates specific reord
        /// </summary>
        /// <param name="hallIn">New record</param>
        /// <returns>Result of operation</returns>
        // PUT: api/Halls/5
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Hall hallIn)
        {
            if (hallIn.Id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var hall = await _hallsRepository.GetAsync(hallIn.Id);

            if (hall == null)
            {
                return NotFound();
            }

            try
            {
                await _hallsRepository.UpdateAsync(hallIn.Id, hallIn);
            }
            catch (Exception)
            {
                throw new Error(Errors.Update_error, $"Can not update record {hall}");
            }
            return NoContent();
        }

        /// <summary>
        /// Delete specific record
        /// </summary>
        /// <param name="id">Id of the record to delete</param>
        /// <returns>Result of the operation</returns>
        // DELETE: api/Halls/123456789987654321123456
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (id.Length < 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }

            var hall = await _hallsRepository.GetAsync(id);

            if (hall == null)
            {
                return NotFound();
            }

            await _hallsRepository.RemoveAsync(hall.Id);
            return NoContent();
        }
    }
}
