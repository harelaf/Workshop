using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
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

        public WSEPShippingHandler(string address, HttpClient httpClient)
        {
            _address=address;
            _httpClient=httpClient;
        }

        public async Task<bool> CancelSupply(string transactionId)
        {
            var request = new StringContent(JsonConvert.SerializeObject(new
            {
                action_type = "cancel_supply",
                transactionId
            }));
            // send request
            using var response = await _httpClient.PostAsync(_address, request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string jsonString = await response.Content.ReadAsStringAsync();
            string body = JsonConvert.DeserializeObject<string>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return (body == "1");
        }

        public async Task<bool> Handshake()
        {
            var request = new StringContent(JsonConvert.SerializeObject(new { action_type = "handshake" }));
            // send request
            using var response = await _httpClient.PostAsync(_address, request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

        public async Task<int> Supply(string name, string address, string city, string country, string zip)
        {
            var request = new StringContent(JsonConvert.SerializeObject(new
            {
                action_type = "supply",
                name,
                address,
                city,
                country,
                zip
            }));
            // send request
            using var response = await _httpClient.PostAsync(_address, request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return -1;
            }
            string jsonString = await response.Content.ReadAsStringAsync();
            string body = JsonConvert.DeserializeObject<string>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return int.Parse(body);
        }
    }
}
