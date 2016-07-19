using System;
using NServiceBus;

public static class ConventionExtensions
{
    #region CustomConvention

    public static void ApplyCustomConventions(this BusConfiguration busConfiguration)
    {
        var conventions = busConfiguration.Conventions();
        conventions.DefiningCommandsAs(t =>
        {
            return t.Namespace != null &&
                   t.Namespace.EndsWith("Commands");
        });
        conventions.DefiningEventsAs(t =>
        {
            return t.Namespace != null &&
                   t.Namespace.EndsWith("Events");
        });
        conventions.DefiningMessagesAs(t =>
        {
            return t.Namespace == "Messages";
        });
        conventions.DefiningEncryptedPropertiesAs(p =>
        {
            return p.Name.StartsWith("Encrypted");
        });
        conventions.DefiningDataBusPropertiesAs(p =>
        {
            return p.Name.EndsWith("DataBus");
        });
        conventions.DefiningExpressMessagesAs(t =>
        {
            return t.Name.EndsWith("Express");
        });
        conventions.DefiningTimeToBeReceivedAs(t =>
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