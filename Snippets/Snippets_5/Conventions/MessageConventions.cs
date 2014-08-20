using System;
using NServiceBus;

public class MessageConventions
{
    public void Simple()
    {
        #region MessageConventionsV5

        var configure = Configure.With(b =>
        {
            b.Conventions().DefiningCommandsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"));
            b.Conventions().DefiningEventsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"));
            b.Conventions().DefiningMessagesAs(t => t.Namespace == "Messages");
            b.Conventions().DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
            b.Conventions().DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            b.Conventions().DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
            b.Conventions().DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);
        });

        #endregion
    }
}