using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tennis.API.Shared.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Shortname { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public Country Country { get; set; } = new Country();
        public string Picture { get; set; } = string.Empty;
        public PlayerData Data { get; set; } = new PlayerData();
    }
}
