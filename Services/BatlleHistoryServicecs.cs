using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Models;
using Pokemon.Services.Interfaces;

namespace Pokemon.Services
{
    public class BattleHistoryService : IBattleHistoryService
    {
        private readonly PokemonDbContext _context;

        public BattleHistoryService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<List<BattleHistory>> GetBattleHistoriesAsync()
        {
            return await _context.BattleHistories
                .OrderByDescending(b => b.Date)
                .ToListAsync();
        }

        public async Task SaveBattleHistoryAsync(BattleHistory battleHistory)
        {
            _context.BattleHistories.Add(battleHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBattleHistoryAsync(int id)
        {
            var battleHistory = await _context.BattleHistories.FindAsync(id);

            if (battleHistory != null)
            {
                _context.BattleHistories.Remove(battleHistory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
