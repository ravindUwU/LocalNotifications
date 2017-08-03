namespace RavinduL.LocalNotifications.Presenters
{
	using System;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Input;
	using Windows.UI.Xaml.Media;
	using Windows.UI.Xaml.Media.Animation;

	/// <summary>
	/// A simple implementation of a <see cref="LocalNotificationPresenter"/> to send text notifications to users.
	/// </summary>
	public sealed class SimpleNotificationPresenter : LocalNotificationPresenter
	{
		#region Dependency properties
		/// <summary>
		/// A sequence of characters from the Segoe MDL2 Assets font that can be displayed alongside the text of the notification.
		/// </summary>
		public string Glyph
		{
			get { return (string)GetValue(GlyphProperty); }
			set { SetValue(GlyphProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="SimpleNotificationPresenter.Glyph"/>.
		/// </summary>
		public static readonly DependencyProperty GlyphProperty =
			DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(SimpleNotificationPresenter), new PropertyMetadata(""));

		/// <summary>
		/// The text contained within the notification.
		/// </summary>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="SimpleNotificationPresenter.Text"/>.
		/// </summary>
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(nameof(Text), typeof(string), typeof(SimpleNotificationPresenter), new PropertyMetadata(""));

		/// <summary>
		/// The logic to be executed when the user activates the notification.
		/// </summary>
		public Action Action { get; set; }
		#endregion

		private DoubleAnimation ShowAnimation;
		private DoubleAnimation HideAnimation;
		private DoubleAnimation RestoreAnimation;

		private Storyboard ShowStoryboard;
		private Storyboard HideStoryboard;
		private Storyboard RestoreStoryboard;

		private Grid LayoutRoot;
		private Grid Target;
		private Button HideButton;

		private TranslateTransform translation
		{
			get { return LayoutRoot.RenderTransform as TranslateTransform; }
			set { LayoutRoot.RenderTransform = value; }
		}

		protected override void OnStateChanged(LocalNotificationState newState, LocalNotificationState previousState)
		{
			if (LayoutRoot != null)
			{
				LayoutRoot.ManipulationMode = (newState == LocalNotificationState.Shown ? ManipulationModes.TranslateY : ManipulationModes.None);
			}
		}

		/// <summary>
		/// Creates an instance of the <see cref="SimpleNotificationPresenter"/> class.
		/// </summary>
		/// <param name="duration">The time duration for which the the notification should persist on the screen.</param>
		/// <param name="text">The text contained within the notification.</param>
		/// <param name="action">The logic to be executed when the user activates the notification.</param>
		/// <param name="glyph">A sequence of characters from the Segoe MDL2 Assets font that can be displayed alongside the text of the notification.</param>
		public SimpleNotificationPresenter(TimeSpan duration, string text, Action action = null, string glyph = null) : base(duration)
		{
			DefaultStyleKey = typeof(SimpleNotificationPresenter);

			Duration = duration;
			Text = text;
			Action = action;
			Glyph = glyph;
		}

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			ShowAnimation.From = HideAnimation.To = -LayoutRoot.ActualHeight;
		}

		protected override void OnApplyTemplate()
		{
			LayoutRoot = (Grid)GetTemplateChild(nameof(LayoutRoot));
			Target = (Grid)GetTemplateChild(nameof(Target));
			HideButton = (Button)GetTemplateChild(nameof(HideButton));

			ShowStoryboard = (Storyboard)GetTemplateChild(nameof(ShowStoryboard));
			HideStoryboard = (Storyboard)GetTemplateChild(nameof(HideStoryboard));
			RestoreStoryboard = (Storyboard)GetTemplateChild(nameof(RestoreStoryboard));

			ShowAnimation = (DoubleAnimation)GetTemplateChild(nameof(ShowAnimation));
			HideAnimation = (DoubleAnimation)GetTemplateChild(nameof(HideAnimation));
			RestoreAnimation = (DoubleAnimation)GetTemplateChild(nameof(RestoreAnimation));

			ShowStoryboard.Completed += (sender, e) => State = LocalNotificationState.Shown;
			HideStoryboard.Completed += (sender, e) => State = LocalNotificationState.Hidden;
			RestoreStoryboard.Completed += (sender, e) => State = LocalNotificationState.Shown;

			LayoutRoot.ManipulationDelta += LayoutRoot_ManipulationDelta;
			LayoutRoot.ManipulationCompleted += LayoutRoot_ManipulationCompleted;

			Target.Tapped += Target_Tapped;

			HideButton.Click += HideButton_Click;
		}

		private void HideButton_Click(object sender, RoutedEventArgs e)
		{
			HideButton.Click -= HideButton_Click;

			Hide();
		}

		private void Target_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Target.Tapped -= Target_Tapped;

			Action?.Invoke();

			Hide();
		}

		private void LayoutRoot_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			if (e.Cumulative.Translation.Y < -40)
			{
				Hide();
			}
			else
			{
				Restore();
			}
		}

		private void LayoutRoot_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			translation.Y += e.Delta.Translation.Y;

			if (translation.Y > 20)
			{
				translation.Y = 20;
			}
		}
		
		protected override void OnShowing()
		{
			ExecuteAfterLoading(ShowStoryboard.Begin);
		}

		protected override void OnHiding()
		{
			HideAnimation.From = translation.Y;
			HideStoryboard.Begin();
		}

		protected override void OnRestoring()
		{
			RestoreAnimation.From = translation.Y;
			RestoreStoryboard.Begin();
		}
	}
}
