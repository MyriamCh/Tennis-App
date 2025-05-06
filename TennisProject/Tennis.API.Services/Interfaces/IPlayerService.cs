using Tennis.API.Shared.Dtos;

namespace Tennis.API.Services.Interfaces;
public interface IPlayerService
{
    Task<IEnumerable<PlayerDto>> GetPlayersSortedByRankAsync();
    Task<PlayerDto?> GetPlayerByIdAsync(int id);
    Task<PlayerStatsDto> GetStatsAsync();
}

