using Pokemon.Models;
using static Pokemon.Models.ApiResponse;

namespace Pokemon.Services.Interfaces
{
    public interface IPokemonService
    {
        Task<Result<Pokemon.Models.Pokemon>> GetPokemonByNameOrId(string nameOrId, CancellationToken cancellationToken = default);
        Task<Result<List<Pokemon.Models.Pokemon>>> GetRandomPokemons(CancellationToken cancellationToken = default);
        Task<Result<PokemonList>> GetPaginatedPokemon(int currentPage, int itemsPerPage = 10, CancellationToken cancellationToken = default);
        Task<Result<List<Pokemon.Models.Pokemon>>> GetFullPokemonDetails(List<PokemonResult> pokemonResults, CancellationToken cancellationToken = default);
        Task<Result<List<string>>> LoadAllPokemonNames(CancellationToken cancellationToken = default);
        Task<Result<List<string>>> GetFilteredPokemonSuggestions(string query, CancellationToken cancellationToken = default);

    }
}