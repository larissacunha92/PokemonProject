using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Models;

namespace Pokemon.Services
{
    public class FavoritePokemonService
    {
        private readonly PokemonDbContext _context;

        public FavoritePokemonService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoritePokemon>> GetFavoritesAsync()
        {
            return await _context.Favorites.ToListAsync();
        }

        public async Task AddFavoriteAsync(int pokemonId, string name, string imageUrl)
        {
            if (!await _context.Favorites.AnyAsync(f => f.PokemonId == pokemonId))
            {
                _context.Favorites.Add(new FavoritePokemon
                {
                    PokemonId = pokemonId,
                    Name = name,
                    ImageUrl = imageUrl
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFavoriteAsync(int pokemonId)
        {
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.PokemonId == pokemonId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
