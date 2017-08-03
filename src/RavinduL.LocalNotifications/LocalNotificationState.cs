namespace RavinduL.LocalNotifications
{
	/// <summary>
	/// Specifies the state of a <see cref="LocalNotificationPresenter"/>.
	/// </summary>
	public enum LocalNotificationState
	{
		/// <summary>
		/// The <see cref="LocalNotificationPresenter"/> being hidden from view, and the user not being able to interact with it.
		/// </summary>
		Hidden,

		/// <summary>
		/// The <see cref="LocalNotificationPresenter"/> transitioning to the <see cref="LocalNotificationState.Hidden"/> state from being shown.
		/// </summary>
		Hiding,

		/// <summary>
		/// The <see cref="LocalNotificationPresenter"/> transitioning to the <see cref="LocalNotificationState.Shown"/> state from being hidden.
		/// </summary>
		Showing,

		/// <summary>
		/// The <see cref="LocalNotificationPresenter"/> being displayed, awaiting user interaction or to be hidden.
		/// </summary>
		Shown,

		/// <summary>
		/// The <see cref="LocalNotificationPresenter"/> transitioning to the <see cref="LocalNotificationState.Shown"/> state, but not from being hidden.
		/// </summary>
		Restoring,
	}
}
