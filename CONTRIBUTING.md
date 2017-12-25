# Contributing

This document contains guidelines to adhere to, when contributing code to this repository. For reporting bugs, making suggestions, etc., please [submit an issue](https://github.com/RavinduL/LocalNotifications/issues/new).

## Code of Conduct

During participation / contribution, make sure to act according to code of conduct located [here](https://github.com/RavinduL/Meta/blob/master/CODE_OF_CONDUCT.md).

## Prerequisites

1. [Visual Studio](https://www.visualstudio.com/) with the Universal Windows Platform development tools.
2. (Recommended) [File Nesting extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.FileNesting) for Visual Studio.
3. (Recommended) [XAML Styler extension](https://marketplace.visualstudio.com/items?itemName=NicoVermeir.XAMLStyler) for Visual Studio.

## Adding new Notifications

New types of notifications (whose use-cases are not too specific) are always welcome to be included in this library. However, please ensure that the following conditions are met,

1. The notification is created according to the [creation guide](https://github.com/RavinduL/LocalNotifications/wiki/Guide:-Creating-Your-Own-Notification).

2. The class representing the notification should,

	1. Be a member of the `RavinduL.LocalNotifications.Notifications` namespace.

	2.	Be separated into multiple files according to the following criteria,

		File                             | Contents
		-------------------------------- | --------
		`NotificationName.cs`            | Code that contains the logic of the notification.
		`NotificationName.Properties.cs` | Dependency properties with their corresponding `static` fields.
		`NotificationName.Constants.cs`  | Notification-level constants (e.g. names of expected template elements).

		-	The main file (`NotificationName.cs`) should declare the class as `public partial NotificationName`, while the others should add to it by referring to it as `partial class NotificationName`.

		-	If possible, the File Nesting extension should be used to nest the other files dependent under main file within Solution Explorer (right click → File Nesting → Nest Item...),

			![Solution explorer](https://i.imgur.com/o9s11fj.png)

	3. Should have the [`TemplatePart`](https://docs.microsoft.com/en-us/dotnet/api/system.windows.templatepartattribute) and [`TemplateVisualState`](https://docs.microsoft.com/en-us/dotnet/api/system.windows.templatevisualstateattribute) attributes containing the names of the expected template elements and visual states, if any.

	4. Have characteristic public members documented using correctly formatted [XML documentation comments](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments).

	5.	Handle changes in values of dependency properties by a method in the main class, like so,

		```C#
		public ... Some
		{
			get { return (...)GetValue(SomeProperty); }
			set { SetValue(SomeProperty, value); }
		}

		public static readonly DependencyProperty SomeProperty =
			DependencyProperty.Register(nameof(Some), typeof(...), typeof(NotificationName), new PropertyMetadata(null, (d, e) => ((NotificationName)d).OnSomeChanged()));
		```

		wherein the `OnSomeChanged` method is declared in `NotificationName.cs`, _and doesn't need to be parameterless_.

3.	The XAML resource dictionary containing the default style and template of the notification is named after the notification (i.e. `NotificationName.xaml`), and should be merged into the resource dictionary at `src/RavinduL.LocalNotifications/Themes/Generic.xaml` as,

	```xml
	<ResourceDictionary Source="ms-appx:///RavinduL.LocalNotifications/Notifications/NotificationName/NotificationName.xaml" />
	```

4. If possible, the XAML is consistently formatted by the XAML Styler extension.

5. All files related to the notification are located within subdirectory of the `src/RavinduL.LocalNotifications/Notifications` folder, named the same as the notification, making the following directory structure,

	```
	src/RavinduL.LocalNotifications/Notifications/
	├─ NotificationName/
	│   ├─ NotificationName.cs
	│   ├─ NotificationName.Properties.cs
	│   ├─ NotificationName.Constants.cs
	│   └─ NotificationName.xaml
	```
