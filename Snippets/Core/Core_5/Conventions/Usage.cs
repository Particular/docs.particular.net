namespace Core5.Conventions
{
    using System;
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region MessageConventions

            var conventions = busConfiguration.Conventions();
            conventions.DefiningCommandsAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages.Commands";
            });
            conventions.DefiningEventsAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages.Events";
            });
            conventions.DefiningMessagesAs(t =>
            {
                return t.Namespace == "MyNamespace.Messages";
            });
            conventions.DefiningEncryptedPropertiesAs(p =>
            {
                return p.Name.StartsWith("Encrypted");
            });
            conventions.DefiningDataBusPropertiesAs(p =>
            {
                return p.Name.EndsWith("DataBus");
            });
            conventions.DefiningExpressMessagesAs(t =>
            {
                return t.Name.EndsWith("Express");
            });
            conventions.DefiningTimeToBeReceivedAs(t =>
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