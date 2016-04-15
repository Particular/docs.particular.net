namespace Snippets5
{
    using NServiceBus;

    class ForInstallationOnReplacement
    {
        ForInstallationOnReplacement(BusConfiguration busConfiguration)
        {
            #region Installers

            busConfiguration.EnableInstallers();

            Bus.Create(busConfiguration);//this will run the installers

            #endregion
        }

    }
}