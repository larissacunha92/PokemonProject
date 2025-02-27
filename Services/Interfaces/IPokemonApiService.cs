using PokemonProject.Models.DTOs;
using static PokemonProject.Models.DTOs.ApiResponse;

namespace PokemonProject.Services.Interfaces
{
    public interface IPokemonApiService
    {
        Task<Result<Models.DTOs.Pokemon>> GetPokemonByNameOrId(string nameOrId, CancellationToken cancellationToken = default);
        Task<Result<List<Models.DTOs.Pokemon>>> GetRandomPokemons(CancellationToken cancellationToken = default);
        Task<Result<PokemonList>> GetPaginatedPokemon(int currentPage, int itemsPerPage = 10, CancellationToken cancellationToken = default);
        Task<Result<List<Models.DTOs.Pokemon>>> GetFullPokemonDetails(List<PokemonResult> pokemonResults, CancellationToken cancellationToken = default);
        Task<Result<List<string>>> LoadAllPokemonNames(CancellationToken cancellationToken = default);
        Task<Result<List<string>>> GetFilteredPokemonSuggestions(string query, CancellationToken cancellationToken = default);

    }
}