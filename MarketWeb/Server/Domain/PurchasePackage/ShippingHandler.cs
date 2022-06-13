using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace MarketWeb.Server.Domain.PurchasePackage
{
    public interface IShippingHandler
    {
        public Task<bool> Handshake();

        public Task<int> Supply(string name, string address, string city, string country, string zip);

        public Task<bool> CancelSupply(string transactionId);
    }

    public class WSEPShippingHandler : IShippingHandler
    {
        private string _address;
        private HttpClient _httpClient;

        public WSEPShippingHandler(string address, HttpClient httpClient = null)
        {
            _address=address;
            if (httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            else
            {
                _httpClient=httpClient;
            }
        }

        public async Task<bool> CancelSupply(string transactionId)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action_type", "cancel_supply"),
                new KeyValuePair<string, string>("transactionId", transactionId)
            });
            // send request
            using var response = await _httpClient.PostAsync(_address, formContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            //string jsonString = await response.Content.ReadAsStringAsync();
            //string body = JsonConvert.DeserializeObject<string>(jsonString, new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Auto
            //});
            return (await response.Content.ReadAsStringAsync() == "1");
        }

        public async Task<bool> Handshake()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action_type", "handshake")
            });
            // send request
            using var response = await _httpClient.PostAsync(_address, formContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

        public async Task<int> Supply(string name, string address, string city, string country, string zip)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action_type", "supply"),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("address", address),
                new KeyValuePair<string, string>("city", city),
                new KeyValuePair<string, string>("country", country),
                new KeyValuePair<string, string>("zip", zip)
            });

            using var response = await _httpClient.PostAsync(_address, formContent);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return -1;
            }

            string result = await response.Content.ReadAsStringAsync();
            
            return int.Parse(result);
        }
    }
}
