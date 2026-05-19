using API.Models;
using API.Repositories;

namespace API.Services;

public sealed class GameService(IGameRepository repository, BotService bot) : IGameService
{
    public Game CreateGame()
    {
        var game = repository.Create();
        return game;
    }

    public Game? GetGame(Guid id) => repository.GetById(id);

    public (Game Game, int? BotCell) MakeMove(Guid id, int cell)
    {
        var game = repository.GetById(id)
            ?? throw new KeyNotFoundException($"Game {id} not found.");

        if (game.Status != GameStatus.InProgress)
            throw new InvalidOperationException("The game is already over.");

        if (game.Board[cell] != CellValue.Empty)
            throw new InvalidOperationException($"Cell {cell} is already occupied.");

        game.Board[cell] = CellValue.X;
        UpdateStatus(game);

        if (game.Status != GameStatus.InProgress)
        {
            repository.Save(game);
            return (game, null);
        }

        int botCell = bot.GetBestMove(game.Board);
        game.Board[botCell] = CellValue.O;
        UpdateStatus(game);

        repository.Save(game);
        return (game, botCell);
    }

    private static void UpdateStatus(Game game)
    {
        var winner = BotService.GetWinner(game.Board);

        if (winner == CellValue.X)
        {
            game.Status = GameStatus.PlayerWon;
            game.FinishedAt = DateTime.UtcNow;
        }
        else if (winner == CellValue.O)
        {
            game.Status = GameStatus.BotWon;
            game.FinishedAt = DateTime.UtcNow;
        }
        else if (Array.TrueForAll(game.Board, c => c != CellValue.Empty))
        {
            game.Status = GameStatus.Draw;
            game.FinishedAt = DateTime.UtcNow;
        }
        // else: still InProgress
    }
}
