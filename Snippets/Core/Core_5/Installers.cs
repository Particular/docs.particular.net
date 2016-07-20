namespace Core5
{
    using NServiceBus;

    class ForInstallationOnReplacement
    {
        ForInstallationOnReplacement(BusConfiguration busConfiguration)
        {
            #region Installers

            busConfiguration.EnableInstallers();
            // this will run the installers
            Bus.Create(busConfiguration);

            #endregion
        }

    }
}