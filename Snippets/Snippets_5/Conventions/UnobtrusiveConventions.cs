using NServiceBus;

#region UnobtrusiveConventionsV5
public class UnobtrusiveConventions : INeedInitialization
{

    public void Customize(ConfigurationBuilder builder)
    {
        builder.Conventions(c => c
            .DefiningCommandsAs(t => t.Namespace != null
                                     && t.Namespace.StartsWith("MyCompany")
                                     && t.Namespace.EndsWith("Commands"))
            .DefiningEventsAs(t => t.Namespace != null
                                   && t.Namespace.StartsWith("MyCompany")
                                   && t.Namespace.EndsWith("Events")));
    }
}
#endregion