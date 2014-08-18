using NServiceBus;

#region UnobtrusiveConventionsV4
public class UnobtrusiveConventions : IWantToRunBeforeConfiguration
{
    public void Init()
    {
        Configure.Instance
            .DefiningCommandsAs(t => t.Namespace != null
                                     && t.Namespace.StartsWith("MyCompany")
                                     && t.Namespace.EndsWith("Commands"))
            .DefiningEventsAs(t => t.Namespace != null
                                   && t.Namespace.StartsWith("MyCompany")
                                   && t.Namespace.EndsWith("Events"));
    }
}
#endregion