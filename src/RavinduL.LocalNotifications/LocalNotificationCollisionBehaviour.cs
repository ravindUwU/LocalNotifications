namespace RavinduL.LocalNotifications
{
	/// <summary>
	/// Specifies the way in which a collision between two local notifications (i.e. attempting to show one which the other still persists) is handled.
	/// </summary>
	public enum LocalNotificationCollisionBehaviour
	{
		/// <summary>
		/// The new notification gets delayed until the expiration of the current notification, following which it will be shown.
		/// </summary>
		Wait,

		/// <summary>
		/// The new notification will trigger the current notification to be hidden, following which it will be shown.
		/// </summary>
		Replace,
	}
}
