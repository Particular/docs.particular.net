namespace Core6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class ForInstallationOnReplacement
    {
        async Task Simple(EndpointConfiguration endpointConfiguration)
        {
            #region Installers

            endpointConfiguration.EnableInstallers();

            //this will run the installers
            await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            #endregion
        }

    }
}