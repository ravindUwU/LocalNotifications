namespace RavinduL.LocalNotifications.Demo
{
	using System;
	using Windows.UI;
	using Windows.UI.Popups;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Media;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class MainPage : Page
    {
		private NotificationService service;

        public MainPage()
        {
            this.InitializeComponent();
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			service = new NotificationService(NotificationPanel);
		}

		private void DefaultButton_Click(object sender, RoutedEventArgs e)
		{
			service.Show("Default", action: async () => await new MessageDialog("Action Invoked.").ShowAsync());
		}

		private void SucessButton_Click(object sender, RoutedEventArgs e)
		{
			service.Show("Success", '\uE170', action: async () => await new MessageDialog("Action Invoked.").ShowAsync(), color: new SolidColorBrush(Colors.DarkGreen));
		}

		private void ErrorButton_Click(object sender, RoutedEventArgs e)
		{
			service.Show("Error", '\uE171', action: async () => await new MessageDialog("Action Invoked.").ShowAsync(), color: new SolidColorBrush(Colors.DarkRed));
		}

		private void HideButton_Click(object sender, RoutedEventArgs e)
		{
			service.Hide();
		}
	}
}
