using System.Net.Http;

namespace GSU.Museum.Shared.Interfaces
{
    public interface IHTTPClientHandlerCreationService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
