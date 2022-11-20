using System.Net.Http;

namespace UltimatR
{
    public class DsHttpClient : HttpClient
    {
        public DsHttpClient(HttpClientHandler handler) : base(handler) { }
    }
}