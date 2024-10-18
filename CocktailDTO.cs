namespace SOA_CA1
{
    public class CocktailDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public string Image { get; set; }
        public string Alcoholic { get; set; }
        public List<string> IngredientsWithMeasurements { get; set; }
    }
}
