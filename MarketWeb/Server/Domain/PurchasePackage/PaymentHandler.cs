using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarketWeb.Server.Domain.PurchasePackage
{
    public interface IPaymentHandler
    {
        public Task<bool> Handshake();

        public Task<int> Pay(string cardNumber, string month, string year, string holder, string ccv, string id);

        public Task<bool> CancelPay(string transactionId);
    }

    public class WSIEPaymentHandler : IPaymentHandler
    {
        private string _address;
        private HttpClient _httpClient;

        public WSIEPaymentHandler(string address, HttpClient httpClient)
        {
            _address=address;
            _httpClient=httpClient;
        }

        public async Task<bool> CancelPay(string transactionId)
        {
            var request = new StringContent(JsonConvert.SerializeObject(new
            {
                action_type = "cancel_pay",
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

        public async Task<int> Pay(string cardNumber, string month, string year, string holder, string ccv, string id)
        {
            var request = new StringContent(JsonConvert.SerializeObject(new { action_type = "pay",
                cardNumber,
                month,
                year,
                holder,
                ccv,
                id
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
