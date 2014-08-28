using System;
using NServiceBus;

public class MessageConventions
{
    public void Simple()
    {
        #region MessageConventionsV5

        var configuration = new BusConfiguration();

        configuration.Conventions().DefiningCommandsAs(t => t.Namespace != null && t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"));
        configuration.Conventions().DefiningEventsAs(t => t.Namespace != null && t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"));
        configuration.Conventions().DefiningMessagesAs(t => t.Namespace != null && t.Namespace == "Messages");
        configuration.Conventions().DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
        configuration.Conventions().DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
        configuration.Conventions().DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
        configuration.Conventions().DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

        #endregion
    }
}