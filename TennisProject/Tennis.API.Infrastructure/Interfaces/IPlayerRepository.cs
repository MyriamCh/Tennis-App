using Tennis.API.Shared.Models;

namespace Tennis.API.Infrastructure.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetPlayersAsync();
    }
}
