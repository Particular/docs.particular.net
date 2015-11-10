using System;
using NServiceBus;

public static class ConventionExtensions
{
    #region CustomConvention

    public static void ApplyCustomConventions(this Configure configure)
    {
        configure.FileShareDataBus(@"..\..\..\DataBusShare\");
        configure.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
        configure.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"));
        configure.DefiningMessagesAs(t => t.Namespace == "Messages");
        configure.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
        configure.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
        configure.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
        configure.DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);
    }

    #endregion
}