using System.Threading.Tasks;

class Usage
{
    async Task Launch()
    {
        #region Launch

        await Particular.PlatformLauncher.Launch();

        #endregion
    }

    async Task ShowConsoleOutput()
    {
        #region ShowConsoleOutput

        await Particular.PlatformLauncher.Launch(showPlatformToolConsoleOutput: true);

        #endregion
    }

    async Task ServicePulseDefaultRoute()
    {
        #region ServicePulseDefaultRoute

        await Particular.PlatformLauncher.Launch(servicePulseDefaultRoute: "/monitoring");

        #endregion
    }
}