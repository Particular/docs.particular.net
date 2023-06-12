namespace Core8.Recoverability.Immediate
{
    using NServiceBus;

    public class Usage
    {
        void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region ImmediateRetriesConfiguration

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(3);
                });

            #endregion
        }

        void Disabling(EndpointConfiguration endpointConfiguration)
        {
            #region DisablingImmediateRetriesConfiguration

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(0);
                });

            #endregion
        }
    }
}