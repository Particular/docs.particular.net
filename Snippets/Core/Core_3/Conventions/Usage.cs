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
            // otherwise it will result in: 'System.InvalidOperationException:
            // "No destination specified for message(s): MessageTypeName"
            configure.DefiningCommandsAs(t => t.Namespace == "MyNamespace.Messages.Commands");
            configure.DefiningEventsAs(t => t.Namespace == "MyNamespace.Messages.Events");
            configure.DefiningMessagesAs(t => t.Namespace == "MyNamespace.Messages");
            configure.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
            configure.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            configure.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
            configure.DefiningTimeToBeReceivedAs(t =>
            t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

            #endregion
        }

    }
}
