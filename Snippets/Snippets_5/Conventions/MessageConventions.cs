using System;
using NServiceBus;

public class MessageConventions
{
    public void Simple()
    {
        #region MessageConventions

        var configuration = new BusConfiguration();
        var conventionsBuilder = configuration.Conventions();
        conventionsBuilder.DefiningCommandsAs(t => t.Namespace != null && t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"));
        conventionsBuilder.DefiningEventsAs(t => t.Namespace != null && t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"));
        conventionsBuilder.DefiningMessagesAs(t => t.Namespace != null && t.Namespace == "Messages");
        conventionsBuilder.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
        conventionsBuilder.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
        conventionsBuilder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
        conventionsBuilder.DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

        #endregion
    }
}