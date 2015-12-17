namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class ForInstallationOnReplacement
    {
        public async Task Simple()
        {
            #region Installers

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EnableInstallers();

            await Endpoint.Start(busConfiguration);//this will run the installers

            #endregion
        }

    }
}