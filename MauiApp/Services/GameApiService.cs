using System.Net.Http.Json;
using System.Text.Json;
using TicTacToe.Maui.Models;

namespace TicTacToe.Maui.Services;

public sealed class GameApiService(HttpClient http)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>POST /games — creates a new game session.</summary>
    public async Task<GameResponse> CreateGameAsync(CancellationToken ct = default)
    {
        var response = await http.PostAsync("games", content: null, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<GameResponse>(JsonOptions, ct))!;
    }

    /// <summary>POST /games/{id}/move — player picks a cell; bot responds in the same call.</summary>
    public async Task<GameResponse> MakeMoveAsync(Guid gameId, int cell, CancellationToken ct = default)
    {
        var response = await http.PostAsJsonAsync($"games/{gameId}/move", new { cell }, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<GameResponse>(JsonOptions, ct))!;
    }

    /// <summary>GET /games/{id} — fetches the latest game state.</summary>
    public async Task<GameResponse> GetGameAsync(Guid gameId, CancellationToken ct = default)
    {
        var response = await http.GetFromJsonAsync<GameResponse>($"games/{gameId}", JsonOptions, ct);
        return response!;
    }
}
