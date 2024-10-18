using Newtonsoft.Json;
namespace SOA_CA1.Services;

using SOA_CA1.Enums;
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

    public async Task<List<Cocktail>> GetAllCocktails()
    {
        string filePath = "cocktails.json";

        if (File.Exists(filePath))
        {
            return await ReadCocktailsFromFileAsync(filePath);
        }
        else
        {
            var allCocktails = await FetchCocktailsFromApi();

            await SaveCocktailsToFileAsync(allCocktails, filePath);

            return allCocktails;
        }
    }

    private async Task<List<Cocktail>> FetchCocktailsFromApi()
    {
        var allCocktails = new List<Cocktail>();
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
                        drink.PopulateIngredientsWithMeasurements();
                        drink.SetAlcoholicType();
                    }
                    allCocktails.AddRange(result.Drinks);
                }
            }
        }

        return allCocktails;
    }

    public async Task SaveCocktailsToFileAsync(List<Cocktail> drinks, string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var drinkDTOs = drinks.Select(drink => new CocktailDTO
        {
            Id = drink.Id,
            Name = drink.Name,
            Instructions = drink.Instructions,
            Image = drink.Image,
            Alcoholic = drink.Alcoholic.ToString(),
            IngredientsWithMeasurements = drink.IngredientsWithMeasurements
        }).ToList();

        using (FileStream createStream = File.Create(filePath))
        {
            await JsonSerializer.SerializeAsync(createStream, drinkDTOs, options);
        }
    }


    public async Task<List<Cocktail>> ReadCocktailsFromFileAsync(string filePath)
    {
        using (FileStream openStream = File.OpenRead(filePath))
        {
            var drinkDTOs = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CocktailDTO>>(openStream);
            return drinkDTOs.Select(dto => new Cocktail
            {
                Id = dto.Id,
                Name = dto.Name,
                Instructions = dto.Instructions,
                Image = dto.Image,
                Alcoholic = Enum.TryParse<AlcoholicType>(dto.Alcoholic, true, out var result) ? result : AlcoholicType.Unknown,
                IngredientsWithMeasurements = dto.IngredientsWithMeasurements
            }).ToList();
        }
    }

    public class CocktailResponse
    {
        [JsonProperty("drinks")]
        public List<Cocktail>? Drinks { get; set; }
    }

}
