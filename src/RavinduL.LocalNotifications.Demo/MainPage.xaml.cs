namespace RavinduL.LocalNotifications.Demo
{
	using Presenters;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Windows.UI;
	using Windows.UI.Popups;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Media;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class MainPage : Page
	{
		private LocalNotificationCollisionBehaviour[] collisionBehaviours;
		private List<Tuple<string, Color>> colors;

		private LocalNotificationManager manager;

		public MainPage()
		{
			InitializeComponent();

			collisionBehaviours = (LocalNotificationCollisionBehaviour[])Enum.GetValues(typeof(LocalNotificationCollisionBehaviour));

			colors = new List<Tuple<string, Color>>();

			var info = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Where((p) => p.PropertyType == typeof(Color)).ToList();

			foreach (var i in info)
			{
				colors.Add(new Tuple<string, Color>(i.Name, (Color)i.GetValue(null)));
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			manager = new LocalNotificationManager(NotificationGrid);

			foreach (var item in colors)
			{
				NotificationBackgroundColorBox.Items.Add(item.Item1);
				NotificationForegroundColorBox.Items.Add(item.Item1);
			}

			NotificationBackgroundColorBox.SelectedItem = nameof(Colors.DarkGreen);
			NotificationForegroundColorBox.SelectedItem = nameof(Colors.White);

			NotificationCollisionBehaviourBox.ItemsSource = collisionBehaviours;
			NotificationCollisionBehaviourBox.SelectedItem = LocalNotificationCollisionBehaviour.Replace;
		}

		private void HideCurrentButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			manager.HideCurrent();
		}

		private void HideAllButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			manager.HideAll();
		}

		private void ShowButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			manager.Show(new SimpleNotificationPresenter
			(
				TimeSpan.FromSeconds(Double.Parse(NotificationDurationBox.Text)), 
				text: NotificationTextBox.Text,
				action: async () => await new MessageDialog(NotificationTextBox.Text).ShowAsync(),
				glyph: "\uE001"
			)
			{
				Background = GetSolidColorBrush(NotificationBackgroundColorBox.SelectedIndex),
				Foreground = GetSolidColorBrush(NotificationForegroundColorBox.SelectedIndex),
			}, 
			(LocalNotificationCollisionBehaviour)NotificationCollisionBehaviourBox.SelectedItem);
		}

		private void NotificationDurationBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			double value;
			ShowButton.IsEnabled = Double.TryParse(NotificationDurationBox.Text, out value);
		}

		private SolidColorBrush GetSolidColorBrush(int selectedIndex)
		{
			return (selectedIndex > -1 ? new SolidColorBrush(colors[selectedIndex].Item2) : new SolidColorBrush(Colors.Transparent));
		}

		private void NotificationBackgroundColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			NotificationBackgroundColorPreview.Background = GetSolidColorBrush(NotificationBackgroundColorBox.SelectedIndex);
		}

		private void NotificationForegroundColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			NotificationForegroundColorPreview.Background = GetSolidColorBrush(NotificationForegroundColorBox.SelectedIndex);
		}
	}
}
