namespace API.Models.Responses;

/// <param name="Id">Unique game identifier.</param>
/// <param name="Board">9-element board: "Empty", "X", or "O".</param>
/// <param name="Status">InProgress | PlayerWon | BotWon | Draw</param>
/// <param name="BotCell">Index of the cell the bot just played, or null if the bot did not move this turn.</param>
/// <param name="CreatedAt">UTC timestamp when the game was created.</param>
/// <param name="FinishedAt">UTC timestamp when the game ended, or null if still in progress.</param>
public record GameResponse(
    Guid Id,
    string[] Board,
    string Status,
    int? BotCell,
    DateTime CreatedAt,
    DateTime? FinishedAt
);
