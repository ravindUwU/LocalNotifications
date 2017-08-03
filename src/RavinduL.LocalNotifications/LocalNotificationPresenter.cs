namespace RavinduL.LocalNotifications
{
	using System;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	
	/// <summary>
	/// The base class for a control that functions as a local notification.
	/// </summary>
	public abstract class LocalNotificationPresenter : Control
	{
		private DispatcherTimer timer;

		/// <summary>
		/// The interval of time for which the <see cref="LocalNotificationPresenter"/> will persist on screen, fully visible to the user, awaiting either interaction or to be hidden. 
		/// </summary>
		public TimeSpan Duration { get; set; }

		/// <summary>
		/// Brings the <see cref="LocalNotificationPresenter"/> into view. 
		/// <remarks>
		/// The <see cref="LocalNotificationPresenter"/> will be assigned the <see cref="LocalNotificationState.Showing"/> state upon invocation of this method. The developer is expected to manually assign the <see cref="LocalNotificationPresenter.State"/> to <see cref="LocalNotificationState.Shown"/> once it is so.
		/// </remarks>
		/// </summary>
		public void Show()
		{
			State = LocalNotificationState.Showing;

			OnShowing();

			timer = new DispatcherTimer
			{
				Interval = Duration,
			};

			timer.Tick += Timer_Tick;

			timer.Start();
		}

		private void Timer_Tick(object sender, object e)
		{
			timer.Tick -= Timer_Tick;

			Hide();
		}

		/// <summary>
		/// Hides the <see cref="LocalNotificationPresenter"/>.
		/// <remarks>
		/// The <see cref="LocalNotificationPresenter"/> will be assigned the <see cref="LocalNotificationState.Hiding"/> state upon invocation of this method. The developer is expected to manually assign the <see cref="LocalNotificationPresenter.State"/> to <see cref="LocalNotificationState.Hidden"/> once it is so.
		/// </remarks>
		/// </summary>
		public void Hide()
		{
			State = LocalNotificationState.Hiding;

			timer.Stop();

			OnHiding();
		}
		
		/// <summary>
		/// Restores the <see cref="LocalNotificationPresenter"/> into view.
		/// <remarks>
		/// The <see cref="LocalNotificationPresenter"/> will be assigned the <see cref="LocalNotificationState.Restoring"/> state upon invocation of this method. The developer is expected to manually assign the <see cref="LocalNotificationPresenter.State"/> to <see cref="LocalNotificationState.Shown"/> once it is so.
		/// </remarks>
		/// </summary>
		public void Restore()
		{
			State = LocalNotificationState.Restoring;

			OnRestoring();
		}

		/// <summary>
		/// Invoked when the <see cref="LocalNotificationPresenter"/> is being shown.
		/// </summary>
		protected abstract void OnShowing();

		/// <summary>
		/// Invoked when the <see cref="LocalNotificationPresenter"/> is being hidden.
		/// </summary>
		protected abstract void OnHiding();

		/// <summary>
		/// Invoked when the <see cref="LocalNotificationPresenter"/> is being restored.
		/// </summary>
		protected abstract void OnRestoring();

		/// <summary>
		/// Creates an instance of the <see cref="LocalNotificationPresenter"/> class.
		/// </summary>
		/// <param name="duration">The time duration for which the local notification will persist on screen.</param>
		public LocalNotificationPresenter(TimeSpan duration)
		{
			Duration = duration;

			Loaded += (sender, e) =>
			{
				loaded = true;

				OnLoaded(sender, e);
			};
		}

		private bool loaded;

		private RoutedEventHandler delayedExecute;

		/// <summary>
		/// Ensures that the specified logic will be executed once the control is loaded and all elements are initialized.
		/// <remarks>
		/// This method could be used to execute logic that relies on elements of the <see cref="LocalNotificationPresenter"/> being properly drawn.
		/// </remarks>
		/// </summary>
		/// <param name="action">The logic to be executed once the control is loaded.</param>
		protected void ExecuteAfterLoading(Action action)
		{
			if (loaded)
			{
				action();
			}
			else
			{
				delayedExecute = (sender, e) =>
				{
					Loaded -= delayedExecute;

					action();
				};

				Loaded += delayedExecute;
			}
		}
		
		/// <summary>
		/// Invoked once the frame is loaded.
		/// </summary>
		protected virtual void OnLoaded(object sender, RoutedEventArgs e)
		{
		}
		
		/// <summary>
		/// Fired when the <see cref="LocalNotificationPresenter.State"/> changes.
		/// </summary>
		public event EventHandler<LocalNotificationState> StateChanged;

		private LocalNotificationState _State;

		/// <summary>
		/// The state that the <see cref="LocalNotificationPresenter"/> is currently in.
		/// </summary>
		public LocalNotificationState State
		{
			get { return _State; }
			protected set
			{
				if (_State != value)
				{
					var p = _State;
					_State = value;

					OnStateChanged(value, p);

					StateChanged?.Invoke(this, value);
				}
			}
		}

		/// <summary>
		/// Fired when the <see cref="LocalNotificationPresenter.State"/> changes.
		/// </summary>
		/// <param name="newState"></param>
		/// <param name="previousState"></param>
		protected virtual void OnStateChanged(LocalNotificationState newState, LocalNotificationState previousState)
		{
		}

		/// <summary>
		/// Invoked just before the notification is displayed.
		/// </summary>
		protected abstract override void OnApplyTemplate();
	}
}
