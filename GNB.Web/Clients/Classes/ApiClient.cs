using System.Net.Http;

namespace GNB.Web.Clients.Classes
{
    public class ApiClient : IApiClient
    {

        private readonly HttpClient HttpClient;

        public ApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
    }
}
