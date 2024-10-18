
using Newtonsoft.Json;
using SOA_CA1.Enums;
using SOA_CA1.Interfaces;

namespace SOA_CA1
{
    public class Cocktail : IEntity
    {
        [JsonProperty("idDrink")]
        public string Id { get; set; }

        [JsonProperty("strDrink")]
        public string Name { get; set; }

        [JsonProperty("strInstructions")]
        public string Instructions { get; set; }

        [JsonProperty("strDrinkThumb")]
        public string Image { get; set; }

        [JsonProperty("strAlcoholic")]
        public string AlcoholicString { get; set; }
        public AlcoholicType Alcoholic { get; set; }
        public List<string> IngredientsWithMeasurements { get; set; } = new List<string>();

        public void PopulateIngredientsWithMeasurements()
        {
            IngredientsWithMeasurements.Clear();

            for (int i = 1; i <= 15; i++)
            {
                string ingredientPropertyName = $"strIngredient{i}";
                string measurePropertyName = $"strMeasure{i}";

                var ingredient = (string)GetType().GetProperty(ingredientPropertyName)?.GetValue(this);
                var measure = (string)GetType().GetProperty(measurePropertyName)?.GetValue(this);

                if (!string.IsNullOrEmpty(ingredient))
                {
                    string combined = string.IsNullOrEmpty(measure) ? ingredient : $"{measure} {ingredient}";
                    IngredientsWithMeasurements.Add(combined);
                }
            }
        }
        public void SetAlcoholicType()
        {
            Alcoholic = Enum.TryParse<AlcoholicType>(AlcoholicString, true, out var result) ? result : AlcoholicType.Unknown;
        }
    }
}
