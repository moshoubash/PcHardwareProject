using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PcHardware.Models;

namespace PcHardware.Services
{
    public class FedExService
    {
        private readonly string _apiKey;
        private readonly string _apiPassword;
        private readonly string _accountNumber;
        private readonly string _meterNumber;
        private readonly string _baseUrl;

        public FedExService(string apiKey, string apiPassword, string accountNumber, string meterNumber, string baseUrl)
        {
            _apiKey = apiKey;
            _apiPassword = apiPassword;
            _accountNumber = accountNumber;
            _meterNumber = meterNumber;
            _baseUrl = baseUrl;
        }

        public async Task<ShippingResponse> CreateShipment(ShippingRequest request)
        {
            using (var client = new HttpClient())
            {
                var jsonRequest = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/ship/v1/shipments")
                {
                    Content = httpContent
                };

                httpRequest.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiKey}:{_apiPassword}"))}");

                var response = await client.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ShippingResponse>(content);
                }
                else
                {
                    return new ShippingResponse
                    {
                        Status = "Error",
                        Message = $"FedEx API error: {response.ReasonPhrase}"
                    };
                }
            }
        }
    }
}
