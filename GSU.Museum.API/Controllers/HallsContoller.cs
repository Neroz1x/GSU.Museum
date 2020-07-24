using System;
using System.Threading.Tasks;
using GSU.Museum.API.Filters;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
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
        private readonly ICompareService _compareService;

        public HallsController(IHallsRepository exhibitRepository, IHallsService hallsService, ICompareService compareService)
        {
            _hallsRepository = exhibitRepository;
            _hallsService = hallsService;
            _compareService = compareService;
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
        public async Task<IActionResult> GetAllAsync(int? hash)
        {
            var halls = await _hallsService.GetAllAsync(Request);
            if (hash != null)
            {
                if(_compareService.IsListEquals(hash.GetValueOrDefault(), halls))
                {
                    return NoContent();
                }
            }
            return Ok(halls);
        }


        /// <summary>
        /// Gets specific record by id
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///     GET: api/Halls/123456789012345678901234
        /// </remarks>
        /// <param name="id">Id of the record</param>
        /// <param name="hash">Hash from client</param>
        /// <returns>Record or not found</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id, int? hash)
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
            if (hash != null)
            {
                if (_compareService.IsEquals(hash.GetValueOrDefault(), hall))
                {
                    return NoContent();
                }
            }
            return Ok(hall);
        }

        /// <summary>
        /// Add record to database
        /// </summary>
        /// <param name="hall">Record to add</param>
        /// <returns>Result of the operation</returns>
        // POST: api/Hall
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
        /// Updates specific record
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
