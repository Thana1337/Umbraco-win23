using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using block_grid_umbraco.Dtos;

namespace block_grid_umbraco.Services
{
    public class EmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EmailService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task SendDataToAzureFunction(EmailDto dto)
        {

            var emailApiUrl = _configuration.GetConnectionString("EmailProviderUrl");

            if (string.IsNullOrEmpty(emailApiUrl) || !Uri.IsWellFormedUriString(emailApiUrl, UriKind.Absolute))
            {
                throw new InvalidOperationException("Invalid email API URL. Ensure it's an absolute URI.");
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(emailApiUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to send email. Status Code: {response.StatusCode}");
            }
        }
    }
}
