using NServiceBus;

public static class ConventionExtensions
{
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
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Messages");
            });
    }
}