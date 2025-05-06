using Microsoft.Extensions.Options;
using System.Text.Json;
using Tennis.API.Infrastructure.Interfaces;
using Tennis.API.Shared.Models;
using Tennis.API.Shared.Options;

namespace Tennis.API.Infrastructure;
public class PlayerRepository : IPlayerRepository
{
    private const string ClientName = "PlayerAPI";
    private readonly IHttpClientFactory _clientFactory;
    private readonly PlayerApiOptions _options;

    public PlayerRepository(IHttpClientFactory clientFactory, IOptions<PlayerApiOptions> options)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    private HttpClient GetClient()
    {
        var client = _clientFactory.CreateClient(ClientName);
        client.BaseAddress ??= new Uri(_options.BaseAddress!);
        return client;
    }
 
    public async Task<List<Player>> GetPlayersAsync()
    {
        var response = await GetClient().GetAsync(_options.JsonEndpoint);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var jsonDoc = await JsonDocument.ParseAsync(stream);
        var playersJson = jsonDoc.RootElement.GetProperty("players").ToString();

        var players = JsonSerializer.Deserialize<List<Player>>(playersJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return players ?? new List<Player>();
    }
}