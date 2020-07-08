using Android.Net;
using GSU.Museum.Droid.Services;
using GSU.Museum.Shared.Interfaces;
using Javax.Net.Ssl;
using System.Net.Http;
using Xamarin.Android.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(HTTPClientHandlerCreationService))]
namespace GSU.Museum.Droid.Services
{
    public class HTTPClientHandlerCreationService : IHTTPClientHandlerCreationService
    {
        public HttpClientHandler GetInsecureHandler()
        {
            return new IgnoreSSLClientHandler();
        }
    }

    internal class IgnoreSSLClientHandler : AndroidClientHandler
    {
        protected override SSLSocketFactory ConfigureCustomSSLSocketFactory(HttpsURLConnection connection)
        {
            return SSLCertificateSocketFactory.GetInsecure(1000, null);
        }

        protected override IHostnameVerifier GetSSLHostnameVerifier(HttpsURLConnection connection)
        {
            return new IgnoreSSLHostnameVerifier();
        }
    }

    internal class IgnoreSSLHostnameVerifier : Java.Lang.Object, IHostnameVerifier
    {
        public bool Verify(string hostname, ISSLSession session)
        {
            return true;
        }
    }
}