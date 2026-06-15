using System.Globalization;

namespace TicTacToe.Maui.Converters;

/// <summary>Returns <c>true</c> when the bound bool is <c>false</c>, and vice-versa.</summary>
public sealed class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool b && !b;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool b && !b;
}
