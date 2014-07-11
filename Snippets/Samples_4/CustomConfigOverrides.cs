using NServiceBus;

public class CustomConfigOverrides
{
    public void Simple()
    {
        // start code CustomConfigOverridesV4

        var configure = Configure.With(AllAssemblies.Except("NotThis.dll"))
            .DefaultBuilder();
        configure.DefineEndpointName("MyEndpointName");
        configure.DefiningEventsAs(type => type.Name.EndsWith("Event"));

        // end code CustomConfigOverridesV4
    }

}