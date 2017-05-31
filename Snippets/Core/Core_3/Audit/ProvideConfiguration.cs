namespace Core3.Audit
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region AuditProvideConfiguration

    class ProvideConfiguration :
        IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            return new UnicastBusConfig
            {
                ForwardReceivedMessagesTo = "targetAuditQueue"
            };
        }
    }

    #endregion
}
