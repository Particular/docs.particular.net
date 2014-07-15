using System;
using NServiceBus;

public class MessageConventions
{
    public void Simple()
    {
        // start code MessageConventionsV4

        var configure = Configure.With()
            .DefaultBuilder()
            .DefiningCommandsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"))
            .DefiningEventsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"))
            .DefiningMessagesAs(t => t.Namespace == "Messages")
            .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
            .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
            .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
            .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

        // end code MessageConventionsV4
    }

}