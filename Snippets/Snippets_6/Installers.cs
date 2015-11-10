namespace Snippets6
{
    using NServiceBus;

    public class ForInstallationOnReplacement
    {
        public void Simple()
        {
            #region Installers

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EnableInstallers();

            Bus.Create(busConfiguration);//this will run the installers

            #endregion
        }

    }
}