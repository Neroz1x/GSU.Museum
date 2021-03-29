using System;
using System.IO;
using System.Threading.Tasks;

namespace GSU.Museum.Shared.Interfaces
{
    interface ICacheLoadingService
    {
        Task<Uri> GetUrlAsync(string language);
        Task<Uri> GetUrlAsync();

        Task WriteCacheAsync(Stream stream, string versionKey, string keyAlias);
    }
}
