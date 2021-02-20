using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GSU.Museum.API.Filters;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Constants;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
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
        /// <param name="languageList">List of laguages to create cache</param>
        /// <param name="savePhotos">Indecates is needed to save photos</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IEnumerable<string> languageList, [FromQuery] bool? savePhotos)
        {
            try
            {
                if (!_cacheService.IsCoorectLanguage(languageList.ToList()))
                {
                    return BadRequest();
                }

                if (savePhotos.HasValue)
                {
                    await _cacheService.CreateCache(Request, languageList ??
                        new List<string>() { LanguageConstants.LanguageDefault }, savePhotos.Value);
                    return Created(string.Empty, null);
                }
                await _cacheService.CreateCache(Request, languageList ?? new List<string>() { LanguageConstants.LanguageDefault });
                return Created(string.Empty, null);
            }
            catch(Exception ex)
            {
                throw new Error() { Info = ex.Message, ErrorCode = Errors.Unhandled_exception };
            }
        }

        /// <summary>
        /// Get text cahce as Key Value pairs
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Cache/language
        /// </remarks>
        /// <param name="language">Language to download cache</param>
        /// <param name="version">Version of client's cache</param>
        /// <returns></returns>
        /// <response code="200">Return stream to download cache</response>
        /// <response code="204">Return NoContent as cache is up to date</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("{language}")]
        public IActionResult GetTextCache(string language, uint? version)
        {
            try
            {
                if (!_cacheService.IsCoorectLanguage(language))
                {
                    return BadRequest();
                }

                Stream stream = null;
                if (version.HasValue)
                {
                    stream = _cacheService.GetCache(language, version.Value);
                    if(stream is null)
                    {
                        return NoContent();
                    }
                }
                else
                {
                    stream = _cacheService.GetCache(language);
                }
                return File(stream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                if(ex is Error)
                {
                    return NotFound();
                }
                throw new Error() { Info = ex.Message, ErrorCode = Errors.Unhandled_exception };
            }
        }

        /// <summary>
        /// Return photos
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET: api/Cache?version
        /// </remarks>
        /// <param name="version">Version of client's cache</param>
        /// <returns></returns>
        /// <response code="200">Return stream to download cache</response>
        /// <response code="204">Return NoContent as cache is up to date</response>
        /// <response code="404">Item not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public IActionResult GetPhotosCache([FromQuery]uint? version)
        {
            try
            {
                Stream stream = null;
                if (version.HasValue)
                {
                    stream = _cacheService.GetCache(version.Value);
                    if (stream is null)
                    {
                        return NoContent();
                    }
                }
                else
                {
                    stream = _cacheService.GetCache();
                }
                return File(stream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                if (ex is Error)
                {
                    return NotFound();
                }

                throw new Error() { Info = ex.Message, ErrorCode = Errors.Unhandled_exception };
            }
        }
    }
}
