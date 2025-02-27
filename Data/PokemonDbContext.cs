using Microsoft.EntityFrameworkCore;
using PokemonProject.Models.Entities;

namespace PokemonProject.Data
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options) { }

        public DbSet<FavoritePokemon> Favorites { get; set; }
        public DbSet<BattleHistory> BattleHistories { get; set; }
    }
}
