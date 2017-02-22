namespace RavinduL.LocalNotifications
{
	using System;
	using Windows.UI;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Media;
	
	public class NotificationService
	{
		public Panel Parent { get; set; }

		private NotificationPresenter 
			Current = null, 
			Next    = null;
		
		/// <summary>
		/// Initializes an instance of the <see cref="NotificationService"/> class. 
		/// </summary>
		/// <param name="parent">A <see cref="Panel"/> in which Notifications will be displayed.</param>
		public NotificationService(Panel parent)
		{
			Parent = parent;
		}

		private void Show(NotificationPresenter presenter)
		{
			Show(presenter.Text, presenter.Glyph, presenter.Duration, presenter.Action, presenter.Color);
		}

		/// <summary>
		/// Displays a new notification.
		/// </summary>
		/// <param name="text">The text to be contained within the notification.</param>
		/// <param name="glyph">An optional glyph to be shown before the text.</param>
		/// <param name="duration">The time duration that the notification will last on screen for.</param>
		/// <param name="action">An action to be invoked when notification is activated.</param>
		/// <param name="color">The color of the notification.</param>
		public void Show(string text, char? glyph = null, TimeSpan? duration = null, Action action = null, SolidColorBrush color = null)
		{
			var presenter = new NotificationPresenter
			{
				Text              = text,
				Glyph             = glyph,
				Duration          = duration ?? TimeSpan.FromSeconds(5),
				VerticalAlignment = VerticalAlignment.Top,
				Action            = action,
				Color             = color ?? new SolidColorBrush(Colors.Gray),
			};

			if (Current == null)
			{
				Current = presenter;

				Current.Hidden += Current_Hidden;

				Parent.Children.Add(Current);
				Current.Show();
			}
			else
			{
				Next = presenter;
				Current.Hide();
			}
		}

		private void Current_Hidden(object sender, System.EventArgs e)
		{
			if (Current != null)
			{
				Parent.Children.Remove(Current);
				Current.Hidden -= Current_Hidden;

				Current = null;
			}

			if (Next != null)
			{
				Show(Next);
				Next = null;
			}
		}
		
		/// <summary>
		/// Hides the active notification, if one does exist.
		/// </summary>
		public void Hide()
		{
			Current?.Hide();
		}
	}
}
