// Launch the Particular Service Platform tools

// Allow the platform tools to use a newer installed .NET patch release.
Environment.SetEnvironmentVariable("DOTNET_ROLL_FORWARD", "LatestPatch");

await Particular.PlatformLauncher.Launch();
