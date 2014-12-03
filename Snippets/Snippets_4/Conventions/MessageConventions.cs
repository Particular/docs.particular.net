using System;
using NServiceBus;

public class MessageConventions
{
    public void Simple()
    {
        #region MessageConventions

        // NOTE: When you're self hosting, `.DefiningXXXAs()` has to be before `.UnicastBus()`, 
        // otherwise you'll get: `System.InvalidOperationException: "No destination specified for message(s): MessageTypeName"

        var configure = Configure.With()
            .DefaultBuilder()
            .DefiningCommandsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"))
            .DefiningEventsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"))
            .DefiningMessagesAs(t => t.Namespace == "Messages")
            .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
            .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
            .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
            .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);
       
        #endregion
    }

}