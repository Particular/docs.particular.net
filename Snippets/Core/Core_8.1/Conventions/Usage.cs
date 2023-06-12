namespace Core8.Conventions
{
    using System;
    using System.Reflection;
    using NServiceBus;

    class Usage
    {
        void MessageConventions(EndpointConfiguration endpointConfiguration)
        {
            #region MessageConventions

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "MyNamespace.Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "MyNamespace.Messages.Events");
            conventions.DefiningMessagesAs(type => type.Namespace == "MyNamespace.Messages");
            conventions.DefiningDataBusPropertiesAs(property => property.Name.EndsWith("DataBus"));
            conventions.DefiningTimeToBeReceivedAs(type => type.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

            #endregion
        }

        void MessageConventionsDual(EndpointConfiguration endpointConfiguration)
        {
            #region MessageConventionsDual

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type =>
                type.Namespace == "MyNamespace.Messages.Commands"
                || typeof(ICommand).IsAssignableFrom(type)
            );
            conventions.DefiningEventsAs(type =>
                type.Namespace == "MyNamespace.Messages.Events"
                || typeof(IEvent).IsAssignableFrom(type)
            );
            conventions.DefiningMessagesAs(type =>
                type.Namespace == "MyNamespace.Messages"
                || typeof(IMessage).IsAssignableFrom(type)
            );
            conventions.DefiningDataBusPropertiesAs(property =>
                property.Name.EndsWith("DataBus")
                || typeof(IDataBusProperty).IsAssignableFrom(property.PropertyType) && typeof(IDataBusProperty) != property.PropertyType
            );
            conventions.DefiningTimeToBeReceivedAs(type =>
            type.Name.EndsWith("Expires")
                    ? TimeSpan.FromSeconds(30)
                    : type.GetCustomAttribute<TimeToBeReceivedAttribute>(false)?.TimeToBeReceived ?? TimeSpan.MaxValue
            );

            #endregion
        }
#pragma warning restore 618
    }
}
