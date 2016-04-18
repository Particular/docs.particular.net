namespace Core5.Forwarding
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    #region ProvideConfigurationForMessageForwarding
    class ProvideConfigurationForMessageForwarding : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            return new UnicastBusConfig
            {
                ForwardReceivedMessagesTo = "destinationQueue@machine"
            };
        }
    }
    #endregion
}
