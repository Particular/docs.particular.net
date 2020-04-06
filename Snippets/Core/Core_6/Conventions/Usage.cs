namespace Core6.Conventions
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
            conventions.DefiningExpressMessagesAs(type => type.Name.EndsWith("Express"));
            conventions.DefiningTimeToBeReceivedAs(type => type.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

            #endregion
        }

        void MessageConventionsDual(EndpointConfiguration endpointConfiguration)
        {
            #region MessageConventionsDual

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type =>
                type.Namespace == "MyNamespace.Messages.Commands"
                || type.IsAssignableFrom(typeof(ICommand))
            );
            conventions.DefiningEventsAs(type =>
                type.Namespace == "MyNamespace.Messages.Events"
                || type.IsAssignableFrom(typeof(IEvent))
            );
            conventions.DefiningMessagesAs(type =>
                type.Namespace == "MyNamespace.Messages"
                || type.IsAssignableFrom(typeof(IMessage))
            );
            conventions.DefiningDataBusPropertiesAs(property =>
                property.Name.EndsWith("DataBus")
                || typeof(IDataBusProperty).IsAssignableFrom(property.PropertyType) && typeof(IDataBusProperty) != property.PropertyType
            );
            conventions.DefiningExpressMessagesAs(type =>
                type.Name.EndsWith("Express")
                || type.GetCustomAttribute<ExpressAttribute>(true) != null
            );
            conventions.DefiningTimeToBeReceivedAs(type =>
                type.Name.EndsWith("Expires")
                    ? TimeSpan.FromSeconds(30)
                    : type.GetCustomAttribute<TimeToBeReceivedAttribute>(false)?.TimeToBeReceived ?? TimeSpan.MaxValue
            );

            #endregion
        }
    }
}