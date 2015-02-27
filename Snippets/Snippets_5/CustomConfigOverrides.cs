using NServiceBus;

public class CustomConfigOverrides
{
    public void Simple()
    {
        #region CustomConfigOverrides

        BusConfiguration configuration = new BusConfiguration();

        configuration.AssembliesToScan(AllAssemblies.Except("NotThis.dll"));
        configuration.Conventions().DefiningEventsAs(type => type.Name.EndsWith("Event"));
        configuration.EndpointName("MyEndpointName");

        #endregion
    }

}