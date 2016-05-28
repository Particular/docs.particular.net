using System;
using NServiceBus;

public static class ConventionExtensions
{
    #region CustomConvention
    public static void ApplyCustomConventions(this EndpointConfiguration endpointConfiguration)
    {
        var conventionsBuilder = endpointConfiguration.Conventions();
        conventionsBuilder.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
        conventionsBuilder.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"));
        conventionsBuilder.DefiningMessagesAs(t => t.Namespace == "Messages");
        conventionsBuilder.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
        conventionsBuilder.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
        conventionsBuilder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
        conventionsBuilder
            .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires")
                ? TimeSpan.FromSeconds(30)
                : TimeSpan.MaxValue
            );
    }
    #endregion
}