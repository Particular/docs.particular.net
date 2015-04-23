using NServiceBus;

public class CustomConfigOverrides
{
    public void Simple()
    {
        #region CustomConfigOverrides

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.AssembliesToScan(AllAssemblies.Except("NotThis.dll"));
        busConfiguration.Conventions().DefiningEventsAs(type => type.Name.EndsWith("Event"));
        busConfiguration.EndpointName("MyEndpointName");

        #endregion
    }

}