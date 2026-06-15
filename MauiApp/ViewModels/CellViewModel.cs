using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TicTacToe.Maui.ViewModels;

public partial class CellViewModel : ObservableObject
{
    public int Index { get; }

    private readonly Func<int, Task> _onTap;

    [ObservableProperty] private string  _symbol          = string.Empty;
    [ObservableProperty] private Color   _backgroundColor = Color.FromArgb("#1e1e3c");
    [ObservableProperty] private Color   _textColor       = Colors.White;
    [ObservableProperty] private bool    _isEnabled       = false;

    public CellViewModel(int index, Func<int, Task> onTap)
    {
        Index  = index;
        _onTap = onTap;
    }

    [RelayCommand]
    private Task Tap() => _onTap(Index);


    /// <summary>Apply the value from the API board array ("Empty", "X", "O").</summary>
    public void Apply(string value)
    {
        switch (value)
        {
            case "X":
                Symbol          = "X";
                BackgroundColor = Color.FromArgb("#0d2340");
                TextColor       = Color.FromArgb("#00cfff");
                IsEnabled       = false;
                break;

            case "O":
                Symbol          = "O";
                BackgroundColor = Color.FromArgb("#40110d");
                TextColor       = Color.FromArgb("#ff6b35");
                IsEnabled       = false;
                break;

            default:
                Symbol          = string.Empty;
                BackgroundColor = Color.FromArgb("#1e1e3c");
                TextColor       = Colors.White;
                break;
        }
    }

    public void Enable()  => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public void Reset()
    {
        Symbol          = string.Empty;
        BackgroundColor = Color.FromArgb("#1e1e3c");
        TextColor       = Colors.White;
        IsEnabled       = false;
    }
}
