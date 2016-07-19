using System;
using NServiceBus;

public static class ConventionExtensions
{
    #region CustomConvention

    public static void ApplyCustomConventions(this Configure configure)
    {
        configure.FileShareDataBus(@"..\..\..\DataBusShare\");
        configure.DefiningCommandsAs(t =>
        {
            return t.Namespace != null &&
                   t.Namespace.EndsWith("Commands");
        });
        configure.DefiningEventsAs(t =>
        {
            return t.Namespace != null &&
                   t.Namespace.EndsWith("Events");
        });
        configure.DefiningMessagesAs(t =>
        {
            return t.Namespace == "Messages";
        });
        configure.DefiningEncryptedPropertiesAs(p =>
        {
            return p.Name.StartsWith("Encrypted");
        });
        configure.DefiningDataBusPropertiesAs(p =>
        {
            return p.Name.EndsWith("DataBus");
        });
        configure.DefiningExpressMessagesAs(t =>
        {
            return t.Name.EndsWith("Express");
        });
        configure.DefiningTimeToBeReceivedAs(t =>
        {
            if (t.Name.EndsWith("Expires"))
            {
                return TimeSpan.FromSeconds(30);
            }
            return TimeSpan.MaxValue;
        });
    }

    #endregion
}