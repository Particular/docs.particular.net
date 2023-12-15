using NServiceBus;
using NServiceBus.Support;

class MachineName
{
    void Old()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region core-8to9-machinename-old
        RuntimeEnvironment.MachineNameAction = () => "MyMachineName";
        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void New()
    {
        #region core-8to9-machinename-new
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        endpointConfiguration.UniquelyIdentifyRunningInstance()
            .UsingHostName("MyMachineName");
        #endregion
    }
}

