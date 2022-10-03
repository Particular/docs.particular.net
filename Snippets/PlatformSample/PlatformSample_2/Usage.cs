namespace Core7
{
    class Usage
    {
        void Launch()
        {
            #region Launch

            Particular.PlatformLauncher.Launch();

            #endregion
        }

        void ShowConsoleOutput()
        {
            #region ShowConsoleOutput

            Particular.PlatformLauncher.Launch(showPlatformToolConsoleOutput: true);

            #endregion
        }

        void ServicePulseDefaultRoute()
        {
            #region ServicePulseDefaultRoute

            Particular.PlatformLauncher.Launch(servicePulseDefaultRoute: "/monitoring");

            #endregion
        }
    }
}
