using System.Threading.Tasks;

namespace GSU.Museum.Shared.Interfaces
{
    interface ICacheLoadingService
    {
        Task LoadLanguageCacheAsync(string language, string status);
        Task LoadPhotosCacheAsync(string status);
    }
}
