namespace RavinduL.LocalNotifications.Converters
{
	using System;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Data;

	/// <summary>
	/// Converts a string to <see cref="Visibility.Visible"/> if it isn't null and doesn't contain whitespace, or returns <see cref="Visibility.Collapsed"/>.
	/// </summary>
	/// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
	public sealed class StringToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (value is string s && !String.IsNullOrWhiteSpace(s)) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
