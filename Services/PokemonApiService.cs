using Pokemon.Models;
using Pokemon.Services.Interfaces;
using System.Security.Cryptography;
using System.Threading;
using static Pokemon.Models.ApiResponse;

namespace Pokemon.Services
{
    public class PokemonApiService : IPokemonApiService
    {
        private readonly HttpClient _client;
        private static List<string>? _cachedPokemonNames;
        private static readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

        public PokemonApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Result<Pokemon.Models.PokemonClass>> GetPokemonByNameOrId(string nameOrId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pokemon = await _client.GetFromJsonAsync<Pokemon.Models.PokemonClass>($"{nameOrId}", cancellationToken);
                return pokemon != null
                    ? Result<Pokemon.Models.PokemonClass>.Success(pokemon)
                    : Result<Pokemon.Models.PokemonClass>.Fail("Pokemon not found.");
            }
            catch (Exception e)
            {
                return Result<Pokemon.Models.PokemonClass>.Fail($"An error occurred while fetching Pokemon data. Error: {e.Message}");
            }
        }

        public async Task<Result<List<Pokemon.Models.PokemonClass>>> GetRandomPokemons(CancellationToken cancellationToken = default)
        {
            try
            {
                var allPokemonResult = await LoadAllPokemonNames(cancellationToken);

                if (!allPokemonResult.IsSuccess || allPokemonResult.Data == null || !allPokemonResult.Data.Any())
                    return Result<List<Pokemon.Models.PokemonClass>>.Fail("Failed to load Pokemon names or no Pokemon available.");

                var pokemonCount = allPokemonResult.Data.Count;
                int half = (pokemonCount + 1) / 2;
                const int maxAttempts = 10;
                int attempts = 0;

                Pokemon.Models.PokemonClass? firstPokemon = null;
                Pokemon.Models.PokemonClass? secondPokemon = null;

                while (attempts < maxAttempts && (firstPokemon == null || secondPokemon == null))
                {
                    if (firstPokemon == null)
                    {
                        int firstId = RandomNumberGenerator.GetInt32(1, half + 1);
                        var firstResult = await GetPokemonByNameOrId(firstId.ToString(), cancellationToken);
                        if (firstResult.IsSuccess)
                        {
                            firstPokemon = firstResult.Data;
                        }
                    }

                    if (secondPokemon == null)
                    {
                        int secondId = RandomNumberGenerator.GetInt32(half + 1, pokemonCount + 1);
                        var secondResult = await GetPokemonByNameOrId(secondId.ToString(), cancellationToken);
                        if (secondResult.IsSuccess)
                        {
                            secondPokemon = secondResult.Data;
                        }
                    }

                    attempts++;
                }

                if (firstPokemon == null || secondPokemon == null)
                {
                    return Result<List<Pokemon.Models.PokemonClass>>.Fail("Failed to retrieve random Pokemon after multiple attempts.");
                }

                var randomPokemons = new List<Pokemon.Models.PokemonClass>
                {
                    firstPokemon,
                    secondPokemon
                };

                return Result<List<Pokemon.Models.PokemonClass>>.Success(randomPokemons);
            }
            catch (Exception e)
            {
                return Result<List<Pokemon.Models.PokemonClass>>.Fail($"An error occurred while fetching random Pokemon. Error: {e.Message}");
            }
        }


        public async Task<Result<PokemonList>> GetPaginatedPokemon(int currentPage, int itemsPerPage = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                int offset = (currentPage - 1) * itemsPerPage;
                var paginatedData = await _client.GetFromJsonAsync<PokemonList>($"?offset={offset}&limit={itemsPerPage}", cancellationToken);

                return paginatedData != null
                    ? Result<PokemonList>.Success(paginatedData)
                    : Result<PokemonList>.Fail("Failed to fetch paginated Pokemon data.");
            }
            catch (Exception e)
            {
                return Result<PokemonList>.Fail($"An error occurred while fetching paginated Pokemon data. Error: {e.Message}");
            }
        }

        public async Task<Result<List<Pokemon.Models.PokemonClass>>> GetFullPokemonDetails(List<PokemonResult> pokemonResults, CancellationToken cancellationToken = default)
        {
            try
            {
                var tasks = pokemonResults.Select(async p =>
                {
                    var result = await GetPokemonByNameOrId(p.Name, cancellationToken);
                    if (!result.IsSuccess)
                    {
                        throw new Exception($"Failed to fetch pokemon details. Error: " + result.ErrorMessage);
                    }
                    return result.Data!;
                });

                var pokemonList = (await Task.WhenAll(tasks)).ToList();
                return Result<List<Pokemon.Models.PokemonClass>>.Success(pokemonList);
            }
            catch (Exception e)
            {
                return Result<List<Pokemon.Models.PokemonClass>>.Fail($"An error occurred while fetching Pokemon details. Error: {e.Message}");
            }
        }

        public async Task<Result<List<string>>> LoadAllPokemonNames(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_cachedPokemonNames != null && _cachedPokemonNames.Any())
                {
                    return Result<List<string>>.Success(_cachedPokemonNames);
                }

                await _cacheLock.WaitAsync(cancellationToken);

                try
                {
                    if (_cachedPokemonNames == null || !_cachedPokemonNames.Any())
                    {
                        var allPokemon = new List<string>();
                        int offset = 0;
                        int limit = 100;
                        bool hasMore = true;

                        while (hasMore)
                        {
                            var response = await GetPaginatedPokemon(offset / limit + 1, limit, cancellationToken);

                            if (!response.IsSuccess)
                            {
                                return Result<List<string>>.Fail(response.ErrorMessage ?? "API call failed.");
                            }

                            if (response.Data!.Results.Any())
                            {
                                allPokemon.AddRange(response.Data.Results.Select(p => char.ToUpper(p.Name[0]) + p.Name.Substring(1))); offset += limit;
                            }
                            else
                            {
                                hasMore = false;
                            }
                        }

                        _cachedPokemonNames = allPokemon;
                    }
                    return Result<List<string>>.Success(_cachedPokemonNames);
                }
                finally
                {
                    _cacheLock.Release();
                }
            }
            catch (Exception e)
            {
                return Result<List<string>>.Fail($"An error occurred while loading Pokemon names. Error: {e.Message}");
            }
        }

        public async Task<Result<List<string>>> GetFilteredPokemonSuggestions(string query, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                    return Result<List<string>>.Success(new List<string>());

                var allPokemonNames = await LoadAllPokemonNames(cancellationToken);
                if (!allPokemonNames.IsSuccess)
                    return Result<List<string>>.Fail(allPokemonNames.ErrorMessage ?? "API call failed.");

                var suggestions = allPokemonNames.Data!
                    .Where(name => name.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                    .Take(10)
                    .ToList();

                return Result<List<string>>.Success(suggestions);
            }
            catch (Exception e)
            {
                return Result<List<string>>.Fail($"An error occurred while filtering Pokemon suggestions. Error: {e.Message}");
            }
        }
    }
}
