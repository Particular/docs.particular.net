using NServiceBus;

class MachineName
{
    void Override()
    {
        #region core-8to9-machinename
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        endpointConfiguration.UniquelyIdentifyRunningInstance()
            .UsingHostName("MyMachineName");
        #endregion
    }
}