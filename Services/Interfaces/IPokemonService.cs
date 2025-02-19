using Pokemon.Models;
using static Pokemon.Models.ApiResponse;

namespace Pokemon.Services.Interfaces
{
    public interface IPokemonService
    {
        Task<Result<Pokemon.Models.Pokemon>> GetPokemonByName(string nameOrId);
        Task<Result<Pokemon.Models.Pokemon>> GetRandomPokemon();
        Task<Result<PokemonList>> GetPaginatedPokemon(int currentPage, int itemsPerPage = 10);
        Task<Result<List<Pokemon.Models.Pokemon>>> GetFullPokemonDetails(List<PokemonResult> pokemonResults);
        Task<Result<List<string>>> LoadAllPokemonNames();
        Task<Result<List<string>>> GetFilteredPokemonSuggestions(string query);

    }
}