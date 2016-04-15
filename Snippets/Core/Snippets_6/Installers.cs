namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class ForInstallationOnReplacement
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region Installers

            endpointConfiguration.EnableInstallers();

            await Endpoint.Start(endpointConfiguration);//this will run the installers

            #endregion
        }

    }
}