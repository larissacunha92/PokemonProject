using System.Text.Json.Serialization;

namespace Pokemon.Models
{
    public class Pokemon
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sprites")]
        public Sprites Sprites { get; set; }

        [JsonPropertyName("stats")]
        public List<Stat> Stats { get; set; }
    }

    public class PokemonList
    {
        [JsonPropertyName("count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("results")]
        public List<PokemonResult> Results { get; set; } = new List<PokemonResult>();
    }

    public class PokemonResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

}