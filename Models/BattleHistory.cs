namespace Pokemon.Models
{
    public class BattleHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Pokemon1Name { get; set; } = string.Empty;
        public string Pokemon2Name { get; set; } = string.Empty;
        public string Pokemon1Picture { get; set; } = string.Empty;
        public string Pokemon2Picture { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
    }
}
