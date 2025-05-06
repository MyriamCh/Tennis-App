using Moq;
using Tennis.API.Infrastructure.Interfaces;
using Tennis.API.Services;
using Tennis.API.Services.Interfaces;
using Tennis.API.Shared.Models;

namespace Tennis.API.Tests
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly IPlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _playerService = new PlayerService(_mockPlayerRepository.Object);
        }

        [Fact(DisplayName = "Should return players sorted by rank in ascending order")]
        public async Task GetPlayersSortedByRankAsync_ShouldReturnSortedPlayers_WhenPlayersExist()
        {
            var players = GetMockPlayers();
            _mockPlayerRepository.Setup(r => r.GetPlayersAsync()).ReturnsAsync(players);

            var result = (await _playerService.GetPlayersSortedByRankAsync()).ToList();
            Assert.Equal(1, result[0].Rank);
            Assert.Equal(2, result[1].Rank);
            Assert.Equal(3, result[2].Rank);
            Assert.Equal(4, result[3].Rank);
        }

        [Fact(DisplayName = "Should return empty list if no players found")]
        public async Task GetPlayersSortedByRankAsync_ShouldReturnEmptyList_WhenNoPlayers()
        {
            _mockPlayerRepository.Setup(r => r.GetPlayersAsync()).ReturnsAsync(new List<Player>());

            var result = await _playerService.GetPlayersSortedByRankAsync();
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should return player DTO when player exists")]
        public async Task GetPlayerByIdAsync_ShouldReturnDto_WhenPlayerExists()
        {
            var players = GetMockPlayers();
            _mockPlayerRepository.Setup(r => r.GetPlayersAsync()).ReturnsAsync(players);

            var result = await _playerService.GetPlayerByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("PlayerA One", result.FullName);
            Assert.Equal("US", result.CountryCode);
        }

        [Fact(DisplayName = "Should return null when player not found")]
        public async Task GetPlayerByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _mockPlayerRepository.Setup(r => r.GetPlayersAsync()).ReturnsAsync(new List<Player>());
            var result = await _playerService.GetPlayerByIdAsync(999);
            Assert.Null(result);
        }

        [Fact(DisplayName = "Should return accurate player stats when data exists")]
        public async Task GetStatsAsync_ShouldReturnValidStats()
        {
            var players = GetMockPlayers();
            _mockPlayerRepository.Setup(r => r.GetPlayersAsync()).ReturnsAsync(players);

            var result = await _playerService.GetStatsAsync();
            Assert.Equal("US", result.BestCountryWinRatio!.CountryCode);
            Assert.True(result.AverageIMC > 0);
            Assert.True(result.HeightMedian > 0);
        }

        [Fact(DisplayName = "Should return default stats when no players available")]
        public async Task GetStatsAsync_ShouldReturnDefaults_WhenNoPlayers()
        {
            _mockPlayerRepository.Setup(r => r.GetPlayersAsync()).ReturnsAsync(new List<Player>());

            var result = await _playerService.GetStatsAsync();

            Assert.Equal("N/A", result!.BestCountryWinRatio!.CountryCode);
            Assert.Equal(0, result.AverageIMC);
            Assert.Equal(0, result.HeightMedian);
        }

        private static List<Player> GetMockPlayers()
        {
            return new List<Player>
        {
            new Player
            {
                Id = 1,
                Firstname = "PlayerA",
                Lastname = "One",
                Country = new Country { Code = "US" },
                Picture = "pic1",
                Data = new PlayerData
                {
                    Rank = 3,
                    Height = 180,
                    Weight = 75000,
                    Last = new() { 1, 1, 0 }
                }
            },
            new Player
            {
                Id = 2,
                Firstname = "PlayerB",
                Lastname = "Two",
                Country = new Country { Code = "FR" },
                Picture = "pic2",
                Data = new PlayerData
                {
                    Rank = 1,
                    Height = 190,
                    Weight = 85000,
                    Last = new() { 1, 0, 0 }
                }
            },
            new Player
            {
                Id = 3,
                Firstname = "PlayerC",
                Lastname = "Three",
                Country = new Country { Code = "US" },
                Picture = "pic3",
                Data = new PlayerData
                {
                    Rank = 2,
                    Height = 170,
                    Weight = 70000,
                    Last = new() { 1, 1, 1 }
                }
            },
            new Player
            {
                Id = 4,
                Firstname = "PlayerD",
                Lastname = "Four",
                Country = new Country { Code = "ES" },
                Picture = "pic4",
                Data = new PlayerData
                {
                    Rank = 4,
                    Height = 185,
                    Weight = 80000,
                    Last = new() { 0, 0, 0 }
                }
            }
        };
        }
    }
}

