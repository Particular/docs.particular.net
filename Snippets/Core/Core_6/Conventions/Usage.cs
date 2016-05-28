namespace Core6.Conventions
{
    using System;
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region MessageConventions
            var conventionsBuilder = endpointConfiguration.Conventions();
            conventionsBuilder.DefiningCommandsAs(t => t.Namespace == "MyNamespace.Messages.Commands");
            conventionsBuilder.DefiningEventsAs(t => t.Namespace == "MyNamespace.Messages.Events");
            conventionsBuilder.DefiningMessagesAs(t => t.Namespace == "MyNamespace.Messages");
            conventionsBuilder.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
            conventionsBuilder.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            conventionsBuilder.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
            conventionsBuilder.DefiningTimeToBeReceivedAs(t =>
                t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

            #endregion
        }
    }
}