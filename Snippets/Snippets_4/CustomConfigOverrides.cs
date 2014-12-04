using NServiceBus;

public class CustomConfigOverrides
{
    public void Simple()
    {
        #region CustomConfigOverrides

        var configure = Configure.With(AllAssemblies.Except("NotThis.dll"))
            .DefaultBuilder();
        configure.DefineEndpointName("MyEndpointName");
        configure.DefiningEventsAs(type => type.Name.EndsWith("Event"));

        #endregion
    }

}