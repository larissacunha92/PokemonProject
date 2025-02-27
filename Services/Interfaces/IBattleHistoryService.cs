using PokemonProject.Models.Entities;
using static PokemonProject.Models.DTOs.ApiResponse;

namespace PokemonProject.Services.Interfaces
{
    public interface IBattleHistoryService
    {
        Task<Result<List<BattleHistory>>> GetBattleHistoriesAsync();
        Task<Result<bool>> SaveBattleHistoryAsync(BattleHistory battleHistory);
        Task<Result<bool>> DeleteBattleHistoryAsync(int id);
    }
}