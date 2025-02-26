using Microsoft.EntityFrameworkCore;
using Pokemon.Models;

namespace Pokemon.Data
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options) { }

        public DbSet<FavoritePokemon> Favorites { get; set; }
        public DbSet<BattleHistory> BattleHistories { get; set; }
    }
}
