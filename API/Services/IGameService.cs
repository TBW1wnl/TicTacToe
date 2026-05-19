using API.Models;

namespace API.Services;

public interface IGameService
{
    Game CreateGame();
    Game? GetGame(Guid id);

    /// <summary>
    /// Applies the player's move, then the bot's response (if the game is still in progress).
    /// </summary>
    /// <returns>
    /// The updated game and the cell index the bot played (null if the bot did not move).
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the move is illegal (game over, cell occupied).
    /// </exception>
    (Game Game, int? BotCell) MakeMove(Guid id, int cell);
}
