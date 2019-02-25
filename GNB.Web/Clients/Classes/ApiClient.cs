using GNB.Web.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GNB.Web.Clients
{
    public class ApiClient : IApiClient
    {

        private readonly HttpClient HttpClient;

        public ApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<string> GetStringRates()
        {
            try
            {
                string data = await HttpClient.GetStringAsync("rate");
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException?.Message, e.InnerException);
            }
        }

        public async Task<string> GetStringTransactions(TransactionModel model = null)
        {
            try
            {
                HttpResponseMessage response;

                if (model == null)
                    response = await HttpClient.PostAsJsonAsync("transaction", new TransactionModel());
                else
                    response = await HttpClient.PostAsJsonAsync("transaction", model);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException("The call was not successful");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException?.Message, e.InnerException);
            }
        }
    }
}
