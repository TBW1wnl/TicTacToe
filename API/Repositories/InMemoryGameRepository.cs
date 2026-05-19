using System.Collections.Concurrent;
using API.Models;

namespace API.Repositories;

/// <summary>
/// Thread-safe in-memory store for game sessions.
/// </summary>
public sealed class InMemoryGameRepository : IGameRepository
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();

    public Game Create()
    {
        var game = new Game();
        _games[game.Id] = game;
        return game;
    }

    public Game? GetById(Guid id) =>
        _games.TryGetValue(id, out var game) ? game : null;

    public void Save(Game game) =>
        _games[game.Id] = game;
}
