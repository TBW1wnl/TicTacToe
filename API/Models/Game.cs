namespace API.Models;

/// <summary>
/// Represents a TicTacToe game session.
/// The board is a flat array of 9 cells indexed 0–8 in row-major order:
///   0 | 1 | 2
///   3 | 4 | 5
///   6 | 7 | 8
/// Player is always X; Bot is always O.
/// </summary>
public class Game
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public CellValue[] Board { get; set; } = new CellValue[9];

    public GameStatus Status { get; set; } = GameStatus.InProgress;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime? FinishedAt { get; set; }
}
