namespace Core3.Conventions
{
    using System;
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region MessageConventions

            // NOTE: When self hosting, '.DefiningXXXAs()' has to be before '.UnicastBus()',
            // otherwise it will result in:
            // 'InvalidOperationException: "No destination specified for message(s): MessageTypeName"
            configure.DefiningCommandsAs(type =>
            {
                return type.Namespace == "MyNamespace.Messages.Commands";
            });
            configure.DefiningEventsAs(type =>
            {
                return type.Namespace == "MyNamespace.Messages.Events";
            });
            configure.DefiningMessagesAs(type =>
            {
                return type.Namespace == "MyNamespace.Messages";
            });
            configure.DefiningEncryptedPropertiesAs(property =>
            {
                return property.Name.StartsWith("Encrypted");
            });
            configure.DefiningDataBusPropertiesAs(property =>
            {
                return property.Name.EndsWith("DataBus");
            });
            configure.DefiningExpressMessagesAs(type =>
            {
                return type.Name.EndsWith("Express");
            });
            configure.DefiningTimeToBeReceivedAs(type =>
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
