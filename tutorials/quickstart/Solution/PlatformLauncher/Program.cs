#region PlatformMain

Console.Title = "Platform Launcher";

// Allow the platform tools to use a newer installed .NET patch release.
Environment.SetEnvironmentVariable("DOTNET_ROLL_FORWARD", "LatestPatch");

// The Particular Service Platform tools are disabled initially.
// To enable them, uncomment the line below:
//await Particular.PlatformLauncher.Launch();

#endregion
