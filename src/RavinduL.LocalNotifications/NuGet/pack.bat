@echo off

del /q *.nupkg

nuget pack ..\RavinduL.LocalNotifications.csproj -Prop Configuration=Release -Verbosity detailed

@echo on