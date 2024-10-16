namespace SOA_CA1.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
public class CocktailService
{
    private readonly HttpClient _httpClient;

    public CocktailService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Example: Get cocktail by name
    public async Task<CocktailResponse> GetCocktailByName(string name)
    {
        var response = await _httpClient.GetAsync($"api/json/v1/1/search.php?s={name}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var cocktailData = JsonSerializer.Deserialize<CocktailResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return cocktailData;
    }
}

public class CocktailResponse
{
    public List<Drink> Drinks { get; set; }
}

public class Drink
{
    public string IdDrink { get; set; }
    public string StrDrink { get; set; }
    public string StrInstructions { get; set; }
    public string StrDrinkThumb { get; set; }
}

