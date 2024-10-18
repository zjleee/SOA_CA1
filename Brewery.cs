using Newtonsoft.Json;
using SOA_CA1.Interfaces;

namespace SOA_CA1
{
    public class Brewery : IEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brewery_type")]
        public string BreweryType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }
    }
}
