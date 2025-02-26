using Pokemon.Models;

namespace Pokemon.Services.Interfaces
{
    public interface IBattleHistoryService
    {
        Task<List<BattleHistory>> GetBattleHistoriesAsync();
        Task SaveBattleHistoryAsync(BattleHistory battleHistory);
        Task DeleteBattleHistoryAsync(int id);
    }
}