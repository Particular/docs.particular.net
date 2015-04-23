using NServiceBus;

#region UnobtrusiveConventions
public class UnobtrusiveConventions : INeedInitialization
{
    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.Conventions()
            .DefiningCommandsAs(t => t.Namespace != null
                                     && t.Namespace.StartsWith("MyCompany")
                                     && t.Namespace.EndsWith("Commands"))
            .DefiningEventsAs(t => t.Namespace != null
                                   && t.Namespace.StartsWith("MyCompany")
                                   && t.Namespace.EndsWith("Events"));
    }
}
#endregion