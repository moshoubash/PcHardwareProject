using Newtonsoft.Json;
using System.Text;

public class FedExService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public FedExService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetShippingRatesAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_configuration["FedEx:BaseUrl"]}/rate/v1");

        var credentials = new
        {
            UserCredential = new
            {
                Key = _configuration["FedEx:ApiKey"],
                Password = _configuration["FedEx:ApiPassword"]
            },
            Account = new
            {
                Number = _configuration["FedEx:AccountNumber"],
                MeterNumber = _configuration["FedEx:MeterNumber"]
            }
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
}
