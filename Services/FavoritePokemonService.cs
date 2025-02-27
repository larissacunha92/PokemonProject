using Microsoft.EntityFrameworkCore;
using PokemonProject.Data;
using PokemonProject.Models.Entities;
using PokemonProject.Services.Interfaces;
using static PokemonProject.Models.DTOs.ApiResponse;

namespace PokemonProject.Services
{
    public class FavoritePokemonService : IFavoritePokemonService
    {
        private readonly PokemonDbContext _context;

        public FavoritePokemonService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<FavoritePokemon>>> GetFavoritesAsync()
        {
            try
            {
                var favorites = await _context.Favorites.ToListAsync();
                return Result<List<FavoritePokemon>>.Success(favorites);
            }
            catch (Exception e)
            {
                return Result<List<FavoritePokemon>>.Fail($"An error occurred while fetching Favorite Pokemon data. Error: {e.Message}");
            }
        }

        public async Task<Result<bool>> AddFavoriteAsync(int pokemonId, string name, string imageUrl)
        {
            try
            {
                _context.Favorites.Add(new FavoritePokemon
                {
                    PokemonId = pokemonId,
                    Name = name,
                    ImageUrl = imageUrl ?? ""
                });

                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Fail($"An error occurred while adding favorite Pokemon. Error: {e.Message}");
            }
        }

        public async Task<Result<bool>> RemoveFavoriteAsync(int pokemonId)
        {
            try
            {
                var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.PokemonId == pokemonId);
                if (favorite != null)
                {
                    _context.Favorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                }

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Fail($"An error occurred while removing a favorite Pokemon. Error: {e.Message}");
            }
        }
    }
}
