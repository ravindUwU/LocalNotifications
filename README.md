# LocalNotifications
Simple local (in-app Notifications) for UWP projects. 

![Demo](images/demo.gif)

## Installation

To install the package via the NuGet package manager, run the following command in the Package Manager Console within Visual Studio

### `Install-Package RavinduL.LocalNotifications`

## Usage
### Step 1: Create an instance of the `NotificationService` class.
The constructor requires one parameter - a `Panel`, into which elements will be added to the XAML Element Tree. 
``` xaml
<!-- In MainPage.xaml -->
<Grid x:Name="NotificationPanel"/>
```

``` c#
// In MainPage.xaml.cs (code-behind for MainPage.xaml)
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
}
```
In order to ensure that Notifications always show above other XAML elements, ensure that `NotificationPanel` overlays all other elements. For example,

``` xaml
<!-- Defined at the root of the <Page> -->
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
	<Grid>
		<!-- All other XAML elements go here -->
	<Grid>
	
	<Grid x:Name="NotificationPanel"/>
</Grid>
```

### Step 2: Use the `NotificationService.Show` method to display local notifications of any kind. 
The `Show` method takes multiple parameters;

1. `text`: A `string` containing what the notification reads. 
2. `glyph` (optional): A `char` from the `Segoe MDL2 Assets` font to be shown next to the notification.  
3. `duration` (optional): A `TimeSpan` representing the time duration for which the Notification will persist on screen. 
4. `action` (optional): An `Action` delegate that will be invoked when a notification is activated (clicked/tapped). 
5. `color` (optional): A `SolidColorBrush` denoting the background color of the Notification.
	
<br/>

* `duration` defaults to `TimeSpan.FromSeconds(5)`.
* `color` defaults to `new SolidColorBrush(Colors.Gray)`.

<br/>

* The current notification will be hidden upon the invocation of `action`. 
* If a notification exists when `NotificationService.Show` is called, it will be hidden.
