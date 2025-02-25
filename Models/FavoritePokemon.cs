namespace Pokemon.Models
{
    public class FavoritePokemon
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
