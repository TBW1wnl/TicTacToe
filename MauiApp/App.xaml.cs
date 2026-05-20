using TicTacToe.Maui.Views;

namespace TicTacToe.Maui;

public partial class App : Application
{
    public App(GamePage gamePage)
    {
        InitializeComponent();
        MainPage = new NavigationPage(gamePage)
        {
            BarBackgroundColor = Color.FromArgb("#0d0d1a"),
            BarTextColor = Colors.White
        };
    }
}
