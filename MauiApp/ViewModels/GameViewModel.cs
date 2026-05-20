using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicTacToe.Maui.Services;

namespace TicTacToe.Maui.ViewModels;

public partial class GameViewModel : ObservableObject
{
    private readonly GameApiService _api;
    private Guid? _gameId;

    [ObservableProperty] private string _statusMessage    = "Ready to play?";
    [ObservableProperty] private bool   _isLoading        = false;
    [ObservableProperty] private bool   _isGameOver       = false;
    [ObservableProperty] private string _newGameLabel     = "New Game";

    /// <summary>
    /// Flat 9-element array of cells, indexed row-major:
    ///   0 | 1 | 2
    ///   3 | 4 | 5
    ///   6 | 7 | 8
    /// </summary>
    public CellViewModel[] Cells { get; }

    public GameViewModel(GameApiService api)
    {
        _api  = api;
        Cells = Enumerable.Range(0, 9)
                          .Select(i => new CellViewModel(i, OnCellTappedAsync))
                          .ToArray();
    }


    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task NewGame(CancellationToken ct)
    {
        IsLoading     = true;
        IsGameOver    = false;
        StatusMessage = "Starting game…";

        foreach (var cell in Cells) cell.Reset();

        try
        {
            var game = await _api.CreateGameAsync(ct);
            _gameId      = game.Id;
            NewGameLabel = "Restart";
            ApplyBoard(game.Board);
            EnableEmptyCells();
            StatusMessage = "Your move — you are X";
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Cancelled.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not connect: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task OnCellTappedAsync(int cellIndex)
    {
        if (_gameId is null || IsLoading || IsGameOver) return;

        IsLoading = true;
        DisableAllCells();
        StatusMessage = "Bot is thinking…";

        try
        {
            var game = await _api.MakeMoveAsync(_gameId.Value, cellIndex);
            ApplyBoard(game.Board);
            HandleStatus(game.Status);
        }
        catch (HttpRequestException ex) when ((int?)ex.StatusCode == 409)
        {
            StatusMessage = "That cell is taken — try another.";
            EnableEmptyCells();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            EnableEmptyCells();
        }
        finally
        {
            IsLoading = false;
        }
    }


    private void ApplyBoard(string[] board)
    {
        for (int i = 0; i < 9; i++)
            Cells[i].Apply(board[i]);
    }

    private void HandleStatus(string status)
    {
        switch (status)
        {
            case "PlayerWon":
                StatusMessage = "You won! Well played!";
                IsGameOver    = true;
                DisableAllCells();
                break;

            case "BotWon":
                StatusMessage = "Bot wins! Better luck next time.";
                IsGameOver    = true;
                DisableAllCells();
                break;

            case "Draw":
                StatusMessage = "It's a draw!";
                IsGameOver    = true;
                DisableAllCells();
                break;

            default:
                StatusMessage = "Your turn!";
                EnableEmptyCells();
                break;
        }
    }

    private void DisableAllCells()
    {
        foreach (var cell in Cells) cell.Disable();
    }

    private void EnableEmptyCells()
    {
        foreach (var cell in Cells)
            if (cell.Symbol == string.Empty) cell.Enable();
    }
}
