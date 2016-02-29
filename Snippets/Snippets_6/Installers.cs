namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class ForInstallationOnReplacement
    {
        public async Task Simple()
        {
            #region Installers

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.EnableInstallers();

            await Endpoint.Start(endpointConfiguration);//this will run the installers

            #endregion
        }

    }
}