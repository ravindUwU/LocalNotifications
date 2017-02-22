namespace RavinduL.LocalNotifications
{
	using System;
	using Windows.UI;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Media;

	public sealed partial class NotificationPresenter : UserControl
	{
		private DispatcherTimer Timer;

		public NotificationPresenter()
		{
			this.InitializeComponent();
		}

		public void Show()
		{
			NotificationGrid.UpdateLayout();
			ShowAnimation.From = -NotificationGrid.ActualHeight;
			ShowStoryboard.Begin();

			Timer = new DispatcherTimer
			{
				Interval = Duration,
			};
			Timer.Tick += Timer_Tick;
			Timer.Start();
		}

		private void Timer_Tick(object sender, object e)
		{
			Hide();
			Timer.Tick -= Timer_Tick;
		}

		public event EventHandler Hidden;

		public void Hide()
		{
			Timer.Stop();
			Timer.Tick -= Timer_Tick;

			HideAnimation.From = ((CompositeTransform)NotificationGrid.RenderTransform).TranslateY;
			HideAnimation.To = -NotificationGrid.ActualHeight;
			HideStoryboard.Begin();

			HideStoryboard.Completed += HideStoryboard_Completed;
		}

		private void Restore()
		{
			RestoreAnimation.From = ((CompositeTransform)NotificationGrid.RenderTransform).TranslateY;
			RestoreStoryboard.Begin();
		}

		private void HideStoryboard_Completed(object sender, object e)
		{
			Hidden?.Invoke(this, EventArgs.Empty);
			HideStoryboard.Completed -= HideStoryboard_Completed;
		}

		#region Text
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(NotificationPresenter), new PropertyMetadata(String.Empty));
		#endregion

		#region Duration
		public TimeSpan Duration
		{
			get { return (TimeSpan)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}

		public static readonly DependencyProperty DurationProperty =
			DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(NotificationPresenter), new PropertyMetadata(0));
		#endregion

		#region Action
		public Action Action
		{
			get { return (Action)GetValue(ActionProperty); }
			set { SetValue(ActionProperty, value); }
		}

		public static readonly DependencyProperty ActionProperty =
			DependencyProperty.Register("Action", typeof(Action), typeof(NotificationPresenter), new PropertyMetadata(null));
		#endregion

		#region Color
		public SolidColorBrush Color
		{
			get { return (SolidColorBrush)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}

		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(NotificationPresenter), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));
		#endregion

		#region Glyph
		public char? Glyph
		{
			get { return (char?)GetValue(GlyphProperty); }
			set { SetValue(GlyphProperty, value); }
		}

		public static readonly DependencyProperty GlyphProperty =
			DependencyProperty.Register("Glyph", typeof(char?), typeof(NotificationPresenter), new PropertyMetadata(null));
		#endregion

		private void NotificationGrid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			Action?.Invoke();
			Hide();
		}

		private void NotificationGrid_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
		{
			CompositeTransform transform = (CompositeTransform)NotificationGrid.RenderTransform;
			transform.TranslateY += e.Delta.Translation.Y;

			if (transform.TranslateY > 20)
			{
				transform.TranslateY = 20;
			}
		}

		private void NotificationGrid_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
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

		private void HideButton_Click(object sender, RoutedEventArgs e)
		{
			Hide();
		}
	}
}
