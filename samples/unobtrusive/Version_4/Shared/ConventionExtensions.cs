using System;
using NServiceBus;

public static class ConventionExtensions
{
    #region CustomConvention
    public static void ApplyCustomConventions(this  Configure configure)
    {
        configure
             .FileShareDataBus(@"..\..\..\DataBusShare\")
             .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
             .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
             .DefiningMessagesAs(t => t.Namespace == "Messages")
             .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
             .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
             .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
             .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue)
         ;


    }
    #endregion
}