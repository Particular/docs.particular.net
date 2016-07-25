namespace Core6.Conventions
{
    using System;
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region MessageConventions
            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type =>
            {
                return type.Namespace == "MyNamespace.Messages.Commands";
            });
            conventions.DefiningEventsAs(type =>
            {
                return type.Namespace == "MyNamespace.Messages.Events";
            });
            conventions.DefiningMessagesAs(type =>
            {
                return type.Namespace == "MyNamespace.Messages";
            });
            conventions.DefiningEncryptedPropertiesAs(property =>
            {
                return property.Name.StartsWith("Encrypted");
            });
            conventions.DefiningDataBusPropertiesAs(property =>
            {
                return property.Name.EndsWith("DataBus");
            });
            conventions.DefiningExpressMessagesAs(type =>
            {
                return type.Name.EndsWith("Express");
            });
            conventions.DefiningTimeToBeReceivedAs(type =>
            {
                if (type.Name.EndsWith("Expires"))
                {
                    return TimeSpan.FromSeconds(30);
                }
                return TimeSpan.MaxValue;
            });

            #endregion
        }
    }
}