using Pokemon.Models;
using Pokemon.Services.Interfaces;
using System.Net.Http.Json;
using static Pokemon.Models.ApiResponse;

namespace Pokemon.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _client;

        public PokemonService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/");
        }

        public async Task<Result<Pokemon.Models.Pokemon>> GetPokemonByName(string nameOrId)
        {
            try
            {
                var pokemon = await _client.GetFromJsonAsync<Pokemon.Models.Pokemon>($"{nameOrId}");
                return pokemon != null
                    ? Result<Pokemon.Models.Pokemon>.Success(pokemon)
                    : Result<Pokemon.Models.Pokemon>.Fail("Pokemon not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Pokemon '{nameOrId}': {ex.Message}");
                return Result<Pokemon.Models.Pokemon>.Fail("An error occurred while fetching Pokemon data.");
            }
        }

        public async Task<Result<Pokemon.Models.Pokemon>> GetRandomPokemon()
        {
            try
            {
                var randomId = new Random().Next(1, 1025);
                return await GetPokemonByName(randomId.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching random Pokemon: {ex.Message}");
                return Result<Pokemon.Models.Pokemon>.Fail("An error occurred while fetching a random Pokemon.");
            }
        }

        public async Task<Result<PokemonList>> GetPaginatedPokemon(int currentPage, int itemsPerPage = 10)
        {
            try
            {
                int offset = (currentPage - 1) * itemsPerPage;
                var paginatedData = await _client.GetFromJsonAsync<PokemonList>($"?offset={offset}&limit={itemsPerPage}");

                return paginatedData != null
                    ? Result<PokemonList>.Success(paginatedData)
                    : Result<PokemonList>.Fail("Failed to fetch paginated Pokemon data.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching paginated Pokemon: {ex.Message}");
                return Result<PokemonList>.Fail("An error occurred while fetching paginated Pokemon data.");
            }
        }

        public async Task<Result<List<Pokemon.Models.Pokemon>>> GetFullPokemonDetails(List<PokemonResult> pokemonResults)
        {
            try
            {
                var tasks = pokemonResults.Select(async p =>
                {
                    var result = await GetPokemonByName(p.Name);
                    return result.IsSuccess ? result.Data! : new Pokemon.Models.Pokemon();
                });

                return Result<List<Pokemon.Models.Pokemon>>.Success((await Task.WhenAll(tasks)).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching full Pokemon details: {ex.Message}");
                return Result<List<Pokemon.Models.Pokemon>>.Fail("An error occurred while fetching Pokemon details.");
            }
        }

        public async Task<Result<List<string>>> LoadAllPokemonNames()
        {
            try
            {
                var allPokemon = new List<string>();
                int offset = 0;
                int limit = 100;
                bool hasMore = true;

                while (hasMore)
                {
                    var response = await GetPaginatedPokemon(offset / limit + 1, limit);
                    if (response.IsSuccess && response.Data!.Results.Any())
                    {
                        allPokemon.AddRange(response.Data.Results
                            .Select(p => char.ToUpper(p.Name[0]) + p.Name.Substring(1)));
                        offset += limit;
                    }
                    else
                    {
                        hasMore = false;
                    }
                }

                return Result<List<string>>.Success(allPokemon);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Pokemon names: {ex.Message}");
                return Result<List<string>>.Fail("An error occurred while loading Pokemon names.");
            }
        }

        public async Task<Result<List<string>>> GetFilteredPokemonSuggestions(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                    return Result<List<string>>.Success(new List<string>());

                var allPokemonNames = await LoadAllPokemonNames();
                if (!allPokemonNames.IsSuccess)
                    return Result<List<string>>.Fail(allPokemonNames.ErrorMessage);

                var suggestions = allPokemonNames.Data!
                    .Where(name => name.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                    .Take(5)
                    .ToList();

                return Result<List<string>>.Success(suggestions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering Pokemon suggestions: {ex.Message}");
                return Result<List<string>>.Fail("An error occurred while filtering Pokemon suggestions.");
            }
        }
    }
}
