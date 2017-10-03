using NServiceBus;

class DoNotCreateQueuesOnInstall
{
    DoNotCreateQueuesOnInstall(EndpointConfiguration endpointConfiguration)
    {
        #region do-not-create-queues-on-install

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.DoNotCreateQueuesOnInstall();

        #endregion
    }
}
