namespace Snippets6.Audit
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region AuditWithCode

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.AuditProcessedMessagesTo("targetAuditQueue");

            #endregion
        }


    }
}