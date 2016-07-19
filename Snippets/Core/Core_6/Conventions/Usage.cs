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
            conventionsBuilder.DefiningCommandsAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages.Commands";
            });
            conventionsBuilder.DefiningEventsAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages.Events";
            });
            conventionsBuilder.DefiningMessagesAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages";
            });
            conventionsBuilder.DefiningEncryptedPropertiesAs(p =>
            {
                return p.Name.StartsWith("Encrypted");
            });
            conventionsBuilder.DefiningDataBusPropertiesAs(p =>
            {
                return p.Name.EndsWith("DataBus");
            });
            conventionsBuilder.DefiningExpressMessagesAs(t =>
            {
                return t.Name.EndsWith("Express");
            });
            conventionsBuilder.DefiningTimeToBeReceivedAs(t =>
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