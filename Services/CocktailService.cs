using Newtonsoft.Json;
namespace SOA_CA1.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


public class CocktailService
{
    private readonly HttpClient _httpClient;

    public CocktailService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Drink>> GetAllCocktails()
    {
        string filePath = "cocktails.json";

        if (File.Exists(filePath))
        {
            // Read from the local JSON file if it exists
            return await ReadCocktailsFromFileAsync(filePath);
        }
        else
        {
            // Fetch from API if the file does not exist
            var allCocktails = await FetchCocktailsFromApi();

            // Save the fetched data to the JSON file
            await SaveCocktailsToFileAsync(allCocktails, filePath);

            return allCocktails;
        }
    }

    private async Task<List<Drink>> FetchCocktailsFromApi()
    {
        var allCocktails = new List<Drink>();
        string baseUrl = "https://www.thecocktaildb.com/api/json/v1/1/search.php?f=";
        string allCharacters = "abcdefghijklmnopqrstuvwxyz123456789";

        foreach (char character in allCharacters)
        {
            var response = await _httpClient.GetAsync(baseUrl + character);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CocktailResponse>(data);
                if (result?.Drinks != null)
                {
                    foreach (var drink in result.Drinks)
                    {
                        // Populate combined ingredients
                        drink.PopulateIngredientsWithMeasurements();
                    }
                    allCocktails.AddRange(result.Drinks);
                }
            }
        }

        return allCocktails;
    }

    public async Task SaveCocktailsToFileAsync(List<Drink> drinks, string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var drinkDTOs = drinks.Select(drink => new DrinkDTO
        {
            Id = drink.Id,
            Name = drink.Name,
            Instructions = drink.Instructions,
            Image = drink.Image,
            Alcoholic = drink.Alcoholic,
            IngredientsWithMeasurements = drink.IngredientsWithMeasurements
        }).ToList();

        using (FileStream createStream = File.Create(filePath))
        {
            await JsonSerializer.SerializeAsync(createStream, drinkDTOs, options);
        }
    }


    public async Task<List<Drink>> ReadCocktailsFromFileAsync(string filePath)
    {
        using (FileStream openStream = File.OpenRead(filePath))
        {
            var drinkDTOs = await System.Text.Json.JsonSerializer.DeserializeAsync<List<DrinkDTO>>(openStream);
            return drinkDTOs.Select(dto => new Drink
            {
                Id = dto.Id,
                Name = dto.Name,
                Instructions = dto.Instructions,
                Image = dto.Image,
                Alcoholic = dto.Alcoholic,
                IngredientsWithMeasurements = dto.IngredientsWithMeasurements
            }).ToList();
        }
    }

    public class CocktailResponse
    {
        [JsonProperty("drinks")]
        public List<Drink>? Drinks { get; set; }
    }

}
