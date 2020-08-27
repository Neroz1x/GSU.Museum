using GSU.Museum.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Interfaces
{
    /// <summary>
    /// Service for ExhibitsController
    /// </summary>
    public interface IExhibitsService
    {
        /// <summary>
        /// Create exhibit to add to database
        /// </summary>
        /// <param name="exhibit">New exhibit from form</param>
        /// <param name="files">Collection of files to upload</param>
        /// <param name="photoDescriptionBe">Photo description in belorussian language</param>
        /// <param name="photoDescriptionEn">Photo description in russian language</param>
        /// <param name="photoDescriptionRu">Photo description in english language</param>
        /// <returns>Exhibit with filled photo info</returns>
        Task<ExhibitViewModel> CreateAsync(ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> photoDescriptionBe, IEnumerable<string> photoDescriptionEn, IEnumerable<string> photoDescriptionRu);

        /// <summary>
        /// Create new exhibit to replace existing one with the same id
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="exhibit">New exhibit from form</param>
        /// <param name="files">Collection of files to upload</param>
        /// <param name="ids">Collection of PhotoInfo ids</param>
        /// <returns>Exhibit with filled photo info</returns>
        Task<ExhibitViewModel> EditArticleAsync(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> ids);

        /// <summary>
        /// Create new exhibit to replace existing one with the same id
        /// </summary>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        /// <param name="exhibit">New exhibit from form</param>
        /// <param name="files">Collection of files to upload</param>
        /// <param name="ids">Collection of PhotoInfo ids</param>
        /// <param name="photoDescriptionBe">Photo description in belorussian language</param>
        /// <param name="photoDescriptionEn">Photo description in russian language</param>
        /// <param name="photoDescriptionRu">Photo description in english language</param>
        /// <returns>Exhibit with filled photo info</returns>
        Task<ExhibitViewModel> EditGalleryAsync(string hallId, string standId, ExhibitViewModel exhibit, IEnumerable<IFormFile> files, IEnumerable<string> ids, IEnumerable<string> photoDescriptionBe, IEnumerable<string> photoDescriptionEn, IEnumerable<string> photoDescriptionRu);
    }
}
