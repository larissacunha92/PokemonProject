using Microsoft.EntityFrameworkCore;
using Pokemon.Models;

namespace Pokemon.Services.Interfaces
{
    public class IFavoritePokemonService
    {
        public interface IFavoritePokemonService
        {
            Task AddFavoriteAsync(int pokemonId, string name, string imageUrl);
            Task<List<FavoritePokemon>> GetFavoritesAsync();
            Task RemoveFavoriteAsync(int pokemonId)
        }
    }
}