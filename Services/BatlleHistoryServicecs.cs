using Microsoft.EntityFrameworkCore;
using PokemonProject.Data;
using PokemonProject.Models.Entities;
using PokemonProject.Services.Interfaces;
using static PokemonProject.Models.DTOs.ApiResponse;

namespace PokemonProject.Services
{
    public class BattleHistoryService : IBattleHistoryService
    {
        private readonly PokemonDbContext _context;

        public BattleHistoryService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<BattleHistory>>> GetBattleHistoriesAsync()
        {
            try
            {
                var battleHistories = await _context.BattleHistories
                        .OrderByDescending(b => b.Date)
                        .ToListAsync();

                return Result<List<BattleHistory>>.Success(battleHistories);
            }
            catch (Exception e)
            {
                return Result<List<BattleHistory>>.Fail($"An error occurred while fetching the Battle History. Error: {e.Message}");
            }
        }

        public async Task<Result<bool>> SaveBattleHistoryAsync(BattleHistory battleHistory)
        {
            try
            {
                _context.BattleHistories.Add(battleHistory);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);

            }
            catch (Exception e)
            {
                return Result<bool>.Fail($"An error occurred while saving the Battle History. Error: {e.Message}");
            }
        }

        public async Task<Result<bool>> DeleteBattleHistoryAsync(int id)
        {
            try
            {
                var battleHistory = await _context.BattleHistories.FindAsync(id);

                if (battleHistory != null)
                {
                    _context.BattleHistories.Remove(battleHistory);
                    await _context.SaveChangesAsync();
                }

                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Fail($"An error occurred while deleting the Battle History. Error: {e.Message}");
            }
        }
    }
}
