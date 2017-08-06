namespace RavinduL.LocalNotifications.Demo
{
	using System;
	using System.Linq;
	using Windows.ApplicationModel;
	using Windows.ApplicationModel.Activation;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	sealed partial class App : Application
	{
		public LocalNotificationManager LocalNotificationManager { get; private set; }
		
		public static new App Current;

		public App()
		{
			Current = this;
			InitializeComponent();
			Suspending += OnSuspending;
		}

		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			// Instead of having the equivalent of <Frame x:Name="rootFrame" /> at the root of our app, we replace it with the equivalent of the following XAML,
			// Note the order in which rootFrame and notificationGrid are added to rootGrid in the C# code (i.e the latter *after* the former) that places the latter above the former.
			//
			//     <Grid x:Name="rootGrid">
			//         <Frame x:Name="rootFrame" />
			//         <Grid x:Name="notificationGrid" />
			//     </Grid>

			Grid rootGrid = Window.Current.Content as Grid;
			Frame rootFrame = rootGrid?.Children.Where((c) => c is Frame).Cast<Frame>().FirstOrDefault();

			if (rootGrid == null)
			{
				rootGrid = new Grid();

				rootFrame = new Frame();
				rootFrame.NavigationFailed += OnNavigationFailed;

				var notificationGrid = new Grid();
				LocalNotificationManager = new LocalNotificationManager(notificationGrid);

				rootGrid.Children.Add(rootFrame);
				rootGrid.Children.Add(notificationGrid);
				
				Window.Current.Content = rootGrid;
			}

			if (!e.PrelaunchActivated)
			{
				if (rootFrame.Content == null)
				{
					rootFrame.Navigate(typeof(MainPage), e.Arguments);
				}

				Window.Current.Activate();
			}
		}

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			deferral.Complete();
		}
	}
}
