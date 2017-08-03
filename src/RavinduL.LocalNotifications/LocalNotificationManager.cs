namespace RavinduL.LocalNotifications
{
	using System;
	using System.Collections.Generic;
	using Windows.UI.Xaml.Controls;

	/// <summary>
	/// Manages <see cref="LocalNotificationPresenter"/>s and associated logic.
	/// </summary>
	public class LocalNotificationManager
	{
		private Grid container;

		private LocalNotificationPresenter current;

		Queue<Tuple<LocalNotificationPresenter, LocalNotificationCollisionBehaviour>> queue;

		/// <summary>
		/// Creates an instance of the <see cref="LocalNotificationManager"/> class.
		/// </summary>
		/// <param name="container">The grid within which all <see cref="LocalNotificationPresenter"/>s will reside.</param>
		public LocalNotificationManager(Grid container)
		{
			this.container = container;
			queue = new Queue<Tuple<LocalNotificationPresenter, LocalNotificationCollisionBehaviour>>();
		}

		/// <summary>
		/// Shows a local notification.
		/// </summary>
		/// <param name="presenter">The control that functions as the notification.</param>
		/// <param name="collisionBehaviour">The way in which collisions are handled for the specified <see cref="LocalNotificationPresenter"/>.</param>
		public void Show(LocalNotificationPresenter presenter, LocalNotificationCollisionBehaviour collisionBehaviour = LocalNotificationCollisionBehaviour.Wait)
		{
			if (current == null)
			{
				current = presenter;

				current.StateChanged += Current_StateChanged;
				current.LayoutUpdated += Current_LayoutUpdated;

				container.Children.Add(current);

				current.UpdateLayout();
			}
			else
			{
				queue.Enqueue(new Tuple<LocalNotificationPresenter, LocalNotificationCollisionBehaviour>(presenter, collisionBehaviour));

				if (collisionBehaviour == LocalNotificationCollisionBehaviour.Replace)
				{
					HideCurrent();
				}
			}
		}

		private void Current_LayoutUpdated(object sender, object e)
		{
			current.LayoutUpdated -= Current_LayoutUpdated;
			current.Show();
		}

		private void Current_StateChanged(object sender, LocalNotificationState state)
		{
			if (state == LocalNotificationState.Hidden)
			{
				current.StateChanged -= Current_StateChanged;

				container.Children.Remove(current);

				current = null;

				if (queue.Count > 0)
				{
					var t = queue.Dequeue();
					Show(t.Item1, t.Item2);
				}
			}
		}

		/// <summary>
		/// Hides the current local notification if there is one.
		/// </summary>
		public void HideCurrent()
		{
			current?.Hide();
		}

		/// <summary>
		/// Hides the current local notification and cancels those that are scheduled for the future. 
		/// </summary>
		public void HideAll()
		{
			queue.Clear();
			HideCurrent();
		}
	}
}
