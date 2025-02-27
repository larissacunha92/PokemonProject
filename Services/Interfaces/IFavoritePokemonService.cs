using PokemonProject.Models.Entities;
using static PokemonProject.Models.DTOs.ApiResponse;

namespace PokemonProject.Services.Interfaces
{
    public interface IFavoritePokemonService
    {
        Task<Result<bool>> AddFavoriteAsync(int pokemonId, string name, string imageUrl);
        Task<Result<List<FavoritePokemon>>> GetFavoritesAsync();
        Task<Result<bool>> RemoveFavoriteAsync(int pokemonId);

    }
}

