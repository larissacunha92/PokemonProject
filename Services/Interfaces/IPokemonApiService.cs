using Pokemon.Models;
using static Pokemon.Models.ApiResponse;

namespace Pokemon.Services.Interfaces
{
    public interface IPokemonApiService
    {
        Task<Result<Pokemon.Models.PokemonClass>> GetPokemonByNameOrId(string nameOrId, CancellationToken cancellationToken = default);
        Task<Result<List<Pokemon.Models.PokemonClass>>> GetRandomPokemons(CancellationToken cancellationToken = default);
        Task<Result<PokemonList>> GetPaginatedPokemon(int currentPage, int itemsPerPage = 10, CancellationToken cancellationToken = default);
        Task<Result<List<Pokemon.Models.PokemonClass>>> GetFullPokemonDetails(List<PokemonResult> pokemonResults, CancellationToken cancellationToken = default);
        Task<Result<List<string>>> LoadAllPokemonNames(CancellationToken cancellationToken = default);
        Task<Result<List<string>>> GetFilteredPokemonSuggestions(string query, CancellationToken cancellationToken = default);

    }
}