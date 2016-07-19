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
            configure.DefiningCommandsAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages.Commands";
            });
            configure.DefiningEventsAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages.Events";
            });
            configure.DefiningMessagesAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages";
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

            #endregion
        }

    }
}
