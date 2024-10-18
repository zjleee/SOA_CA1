using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SOA_CA1.Services
{
    public class BreweryService
    {
        private readonly HttpClient _httpClient;

        public BreweryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Brewery>> GetAllBreweries()
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://api.openbrewerydb.org/v1/");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await _httpClient.GetAsync("breweries");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Brewery>>(data);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
    }
}
