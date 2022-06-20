using Newtonsoft.Json;
using System.Collections.Generic;
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

        public WSIEPaymentHandler(string address, HttpClient httpClient = null)
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

        public async Task<bool> CancelPay(string transactionId)
        {
            var formContent = new FormUrlEncodedContent(new[]
                        {
                new KeyValuePair<string, string>("action_type", "cancel_pay"),
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

        public async Task<int> Pay(string cardNumber, string month, string year, string holder, string ccv, string id)
        {
            var formContent = new FormUrlEncodedContent(new[]
                        {
                new KeyValuePair<string, string>("action_type", "pay"),
                new KeyValuePair<string, string>("card_number", cardNumber),
                new KeyValuePair<string, string>("month", month),
                new KeyValuePair<string, string>("year", year),
                new KeyValuePair<string, string>("holder", holder),
                new KeyValuePair<string, string>("ccv", ccv),
                new KeyValuePair<string, string>("id", id)
            });

            using var response = await _httpClient.PostAsync(_address, formContent);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return -1;
            }
            //string jsonString = await response.Content.ReadAsStringAsync();
            //string body = JsonConvert.DeserializeObject<string>(jsonString, new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Auto
            //});
            //return int.Parse(body);
            return int.Parse(await response.Content.ReadAsStringAsync());
        }
    }
}
