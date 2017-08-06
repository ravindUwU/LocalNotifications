namespace RavinduL.LocalNotifications.Demo
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using RavinduL.LocalNotifications.Notifications;
	using Windows.UI;
	using Windows.UI.Popups;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Media;

	public sealed partial class MainPage : Page
	{
		private IEnumerable<Position> positions = new[] { Position.Top, Position.Bottom, };
		private Dictionary<string, SolidColorBrush> colors = new Dictionary<string, SolidColorBrush>();
		private Dictionary<string, char> glyphs = new Dictionary<string, char>();

		public MainPage()
		{
			InitializeComponent();

			var _colors = typeof(Colors)
				.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
				.Where((p) => p.PropertyType == typeof(Color))
				.Select((p) => new Tuple<string, SolidColorBrush>(p.Name, new SolidColorBrush((Color)p.GetValue(p))));

			foreach (var item in _colors)
			{
				colors.Add(item.Item1, item.Item2);
			}

			var _glyphs = typeof(SegoeMDL2Assets)
				.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
				.Where((f) => f.FieldType == typeof(char))
				.OrderBy((f) => f.Name)
				.Select((f) => new Tuple<string, char>(f.Name, (char)f.GetValue(f)));

			glyphs.Add("None", '\0');

			foreach (var item in _glyphs)
			{
				glyphs.Add(item.Item1, item.Item2);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			foreach (var color in colors)
			{
				BackgroundComboBox.Items.Add(color.Key);
				ForegroundComboBox.Items.Add(color.Key);
			}

			foreach (var position in positions)
			{
				PositionComboBox.Items.Add(position.ToString());
			}

			foreach (var glyph in glyphs)
			{
				GlyphComboBox.Items.Add(glyph.Key);
			}

			GlyphComboBox.SelectedItem = "None";
			BackgroundComboBox.SelectedItem = nameof(Colors.DarkGreen);
			ForegroundComboBox.SelectedItem = nameof(Colors.White);
			PositionComboBox.SelectedItem = Position.Top.ToString();
		}

		private Position SelectedPosition => PositionComboBox.SelectedItem.ToString() == Position.Top.ToString() ? Position.Top : Position.Bottom;

		private void ShowButton_Click(object sender, RoutedEventArgs e)
		{
			double duration = Double.NaN;
			Double.TryParse(DurationTextBox.Text, out duration);

			App.Current.LocalNotificationManager.Show(new SimpleNotification
			{
				Text = TextTextBox.Text,
				TimeSpan = Double.IsNaN(duration) ? null as TimeSpan? : TimeSpan.FromSeconds(duration),
				VerticalAlignment = SelectedPosition == Position.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Top,
				Glyph = GlyphTextBlock.Text,
				Background = colors[BackgroundComboBox.SelectedItem.ToString()],
				Foreground = colors[ForegroundComboBox.SelectedItem.ToString()],
				Padding = new Thickness(20),
				Action = async () => await new MessageDialog("Notification invoked.").ShowAsync(),
			});
		}

		private void HideCurrentButton_Click(object sender, RoutedEventArgs e)
			=> App.Current.LocalNotificationManager.HideCurrent();

		private void HideAllButton_Click(object sender, RoutedEventArgs e)
			=> App.Current.LocalNotificationManager.HideAll();

		private void BackgroundComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
			=> BackgroundEllipse.Fill = BackgroundComboBox.SelectedIndex > -1 ? colors[BackgroundComboBox.SelectedItem.ToString()] : new SolidColorBrush(Colors.Transparent);

		private void ForegroundComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
			=> ForegroundEllipse.Fill = ForegroundComboBox.SelectedIndex > -1 ? colors[ForegroundComboBox.SelectedItem.ToString()] : new SolidColorBrush(Colors.Transparent);

		private void GlyphComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
			=> GlyphTextBlock.Text = glyphs[GlyphComboBox.SelectedItem.ToString()].ToString();
	}
}
