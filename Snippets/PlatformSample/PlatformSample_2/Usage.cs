namespace Core7
{
    class Usage
    {
        async Task Launch()
        {
            #region Launch

            await Particular.PlatformLauncher.Launch()
                .ConfigureAwait(false);

            #endregion
        }

        async Task ShowConsoleOutput()
        {
            #region ShowConsoleOutput

            await Particular.PlatformLauncher.Launch(showPlatformToolConsoleOutput: true)
                .ConfigureAwait(false);

            #endregion
        }

        async Task ServicePulseDefaultRoute()
        {
            #region ServicePulseDefaultRoute

            await Particular.PlatformLauncher.Launch(servicePulseDefaultRoute: "/monitoring")
                .ConfigureAwait(false);

            #endregion
        }
    }
}
