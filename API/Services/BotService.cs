using API.Models;

namespace API.Services;

/// <summary>
/// Stateless bot that plays as O using the minimax algorithm.
/// Minimax guarantees the bot never loses: it will win if possible, otherwise draw.
/// </summary>
public sealed class BotService
{
    private static readonly int[][] WinLines =
    [
        [0, 1, 2], [3, 4, 5], [6, 7, 8], // rows
        [0, 3, 6], [1, 4, 7], [2, 5, 8], // columns
        [0, 4, 8], [2, 4, 6]             // diagonals
    ];

    /// <summary>
    /// Returns the best cell index (0–8) for the bot to play on the given board.
    /// </summary>
    public int GetBestMove(CellValue[] board)
    {
        int bestScore = int.MinValue;
        int bestCell = -1;

        var boardCopy = (CellValue[])board.Clone();

        for (int i = 0; i < 9; i++)
        {
            if (boardCopy[i] != CellValue.Empty)
                continue;

            boardCopy[i] = CellValue.O;
            int score = Minimax(boardCopy, depth: 1, isMaximizing: false);
            boardCopy[i] = CellValue.Empty;

            if (score > bestScore)
            {
                bestScore = score;
                bestCell = i;
            }
        }

        return bestCell;
    }

    /// <summary>
    /// Evaluates the board recursively.
    /// O (bot) is the maximizer; X (player) is the minimizer.
    /// Score is adjusted by depth so the bot prefers faster wins.
    /// </summary>
    private static int Minimax(CellValue[] board, int depth, bool isMaximizing)
    {
        var winner = GetWinner(board);

        if (winner == CellValue.O) return 10 - depth;
        if (winner == CellValue.X) return depth - 10;
        if (IsBoardFull(board))    return 0;

        if (isMaximizing)
        {
            int best = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != CellValue.Empty) continue;
                board[i] = CellValue.O;
                best = Math.Max(best, Minimax(board, depth + 1, false));
                board[i] = CellValue.Empty;
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != CellValue.Empty) continue;
                board[i] = CellValue.X;
                best = Math.Min(best, Minimax(board, depth + 1, true));
                board[i] = CellValue.Empty;
            }
            return best;
        }
    }

    /// <summary>Returns the winning player, or Empty if no winner yet.</summary>
    public static CellValue GetWinner(CellValue[] board)
    {
        foreach (var line in WinLines)
        {
            var a = board[line[0]];
            if (a != CellValue.Empty && a == board[line[1]] && a == board[line[2]])
                return a;
        }
        return CellValue.Empty;
    }

    private static bool IsBoardFull(CellValue[] board) =>
        Array.TrueForAll(board, c => c != CellValue.Empty);
}
