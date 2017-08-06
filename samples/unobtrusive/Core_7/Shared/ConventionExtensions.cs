using System;
using NServiceBus;

public static class ConventionExtensions
{
    #region CustomConvention

    public static void ApplyCustomConventions(this EndpointConfiguration endpointConfiguration)
    {
        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningCommandsAs(
            type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Commands");
            });
        conventions.DefiningEventsAs(
            type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Events");
            });
        conventions.DefiningMessagesAs(
            type =>
            {
                return type.Namespace == "Messages";
            });
        conventions.DefiningDataBusPropertiesAs(
            property =>
            {
                return property.Name.EndsWith("DataBus");
            });
        conventions.DefiningExpressMessagesAs(
            type =>
            {
                return type.Name.EndsWith("Express");
            });
        conventions.DefiningTimeToBeReceivedAs(
            type =>
            {
                if (type.Name.EndsWith("Expires"))
                {
                    return TimeSpan.FromSeconds(30);
                }
                return TimeSpan.MaxValue;
            });
    }

    #endregion
}