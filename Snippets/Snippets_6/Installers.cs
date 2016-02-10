namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class ForInstallationOnReplacement
    {
        public async Task Simple()
        {
            #region Installers

            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.EnableInstallers();

            await Endpoint.Start(configuration);//this will run the installers

            #endregion
        }

    }
}