namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class SendOnly
    {
        public async Task Simple()
        {
            #region SendOnly

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.SendOnly();
            IEndpointInstance endpointInstance = await Endpoint.Start(busConfiguration);

            #endregion
        }

    }
}