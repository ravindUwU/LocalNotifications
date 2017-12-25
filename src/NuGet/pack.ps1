Write-Host "Removing *.nupkg in $PWD."
Remove-Item *.nupkg

if (Test-Path "./nuget.exe") {
	$project = "RavinduL.LocalNotifications"

	if (Test-Path "../$project/bin/Release") {
		.\nuget.exe pack "$project.nuspec" -Verbosity Detailed
	} else {
		Write-Host "Build the $project project in the Release (Any CPU) configuration first."
	}
} else {
	Write-Host "The NuGet binary wasn't found in $PWD.`nShould the latest version be downloaded?"

	if ((Read-Host -Prompt "Yes (Y) or No (N)?") -eq "y") {
		Invoke-WebRequest "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile "nuget.exe"
		Write-Host "The NuGet binary was downloaded. Re-run this script."
	}
}
