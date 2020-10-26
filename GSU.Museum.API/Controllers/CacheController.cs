using System;
using System.IO;
using System.Threading.Tasks;
using GSU.Museum.API.Filters;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSU.Museum.API.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// Creates cache
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Cache
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create()
        {
            try
            {
                _cacheService.CreateCache(Request).Wait();
            }
            catch(Exception ex)
            {
                throw new Error() { Info = ex.Message, ErrorCode = Errors.Unhandled_exception };
            }
            return Ok();
        }

        //[HttpGet("GetDB")]
        //public async Task<IActionResult> GetDB(int? version)
        //{
        //    //if(version != null)
        //    //{
        //    //    if(await _cacheService.IsUpToDate(version.Value, Request))
        //    //    {
        //    //        return NoContent();
        //    //    }
        //    //}
        //    Stream stream = _cacheService.GetCahceDB(Request);

        //    if (stream == null)
        //    {
        //        return NotFound();
        //    }

        //    return File(stream, "application/octet-stream");
        //}

        [HttpGet("GetDBSHM")]
        public IActionResult GetDBSHM()
        {
            Stream stream = _cacheService.GetCahceDBSHM(Request);

            if (stream == null)
            {
                return NotFound();
            }

            return File(stream, "application/octet-stream");
        }

        [HttpGet("GetDBWAL")]
        public IActionResult GetDBWAL()
        {
            FileStream stream = _cacheService.GetCahceDBWAL(Request);

            if (stream == null)
            {
                return NotFound();
            }

            return File(stream, "application/octet-stream");
        }
    }
}
