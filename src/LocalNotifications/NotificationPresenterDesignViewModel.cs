namespace RavinduL.LocalNotifications
{
	using Windows.UI;
	using Windows.UI.Xaml.Media;

	class NotificationPresenterDesignViewModel
	{
		public string Text           => "Lorem ipsum dolor sit amet";
		public SolidColorBrush Color => new SolidColorBrush(Colors.DarkGreen);
		public char? Glyph           => '\uE170';
	}
}
