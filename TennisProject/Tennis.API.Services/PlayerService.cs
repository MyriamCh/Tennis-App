using Tennis.API.Infrastructure.Interfaces;
using Tennis.API.Services.Interfaces;
using Tennis.API.Shared.Dtos;
using Tennis.API.Shared.Models;

namespace Tennis.API.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<IEnumerable<PlayerDto>> GetPlayersSortedByRankAsync()
        {
            var players = await _repository.GetPlayersAsync();
            return players.OrderBy(p => p.Data.Rank).Select(MapToDto);
        }
        public async Task<PlayerDto?> GetPlayerByIdAsync(int id)
        {
            var players = await _repository.GetPlayersAsync();
            var player = players.FirstOrDefault(p => p.Id == id);
            return player is null ? null : MapToDto(player);
        }
        public async Task<PlayerStatsDto> GetStatsAsync()
        {
            var players = await _repository.GetPlayersAsync();

            return new PlayerStatsDto
            {
                BestCountryWinRatio = GetBestCountryWinRatio(players),
                AverageIMC = GetAverageIMC(players),
                HeightMedian = GetHeightMedian(players)
            };
        }
        private CountryStatsResult GetBestCountryWinRatio(IEnumerable<Player> players)
        {
            var bestCountry= players
               .GroupBy(p => p.Country.Code)
               .Select(g =>
               {
                   double totalMatches = g.Sum(p => p.Data.Last.Count);
                   double totalWins = g.Sum(p => p.Data.Last.Count(match => match == 1));

                   return new CountryStatsResult
                   {
                       CountryCode = g.Key,
                       WinRatio = totalMatches > 0 ? totalWins / totalMatches : 0
                   };
               })
               .OrderByDescending(g => g.WinRatio)
               .FirstOrDefault();

            return bestCountry ?? new CountryStatsResult { CountryCode = "N/A", WinRatio = 0 };
        }
        private double GetAverageIMC(IEnumerable<Player> players)
        {
            var validIMCs = players
              .Where(p => p.Data.Height > 0 && p.Data.Weight > 0)
             .Select(p =>
             {
                 double weightInKg = p.Data.Weight / 1000.0;
                 double heightInMeters = p.Data.Height / 100.0;
                 return  weightInKg / Math.Pow(heightInMeters, 2);
             }).ToList();

            return validIMCs.Any() ? Math.Round(validIMCs.Average(), 2) : 0;
        }
        private double GetHeightMedian(IEnumerable<Player> players)
        {
            var heights = players.Select(p => p.Data.Height).OrderBy(h => h).ToList();
            int count = heights.Count;

            if (count == 0) return 0;

            return count % 2 == 0
                ? (heights[count / 2 - 1] + heights[count / 2]) / 2.0
                : heights[count / 2];
        }
        private static PlayerDto MapToDto(Player p)
        {
            return new PlayerDto
            {
                FullName = $"{p.Firstname} {p.Lastname}",
                ShortName = p.Shortname,
                Sex = p.Sex,
                Age = p.Data.Age,
                CountryCode = p.Country.Code,
                Rank = p.Data.Rank,
                Picture = p.Picture
            };
        }
    }


}

