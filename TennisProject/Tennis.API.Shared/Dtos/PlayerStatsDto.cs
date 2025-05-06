namespace Tennis.API.Shared.Dtos
{
    public class PlayerStatsDto
    {
        public CountryStatsResult? BestCountryWinRatio { get; set; }
        public double AverageIMC { get; set; }
        public double HeightMedian { get; set; }
    }
    public class CountryStatsResult
    {
        public string? CountryCode { get; set; }
        public double WinRatio { get; set; }
    }
}
