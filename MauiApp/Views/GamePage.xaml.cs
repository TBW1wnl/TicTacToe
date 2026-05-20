using TicTacToe.Maui.ViewModels;

namespace TicTacToe.Maui.Views;

public partial class GamePage : ContentPage
{
    public GamePage(GameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
