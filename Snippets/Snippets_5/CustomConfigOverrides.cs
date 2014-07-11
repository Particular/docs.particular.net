using NServiceBus;

public class CustomConfigOverrides
{
    public void Simple()
    {
        // start code CustomConfigOverridesV5
        
        var configure = Configure.With(b =>
        {
            b.AssembliesToScan(AllAssemblies.Except("NotThis.dll"));
            b.Conventions(c => c.DefiningEventsAs(type => type.Name.EndsWith("Event")));
            b.EndpointName("MyEndpointName");
            b.EndpointVersion("1.2.3");
        });

        // end code CustomConfigOverridesV5
    }

}