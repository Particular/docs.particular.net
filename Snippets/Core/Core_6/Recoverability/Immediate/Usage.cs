namespace Core6.Recoverability.Immediate
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
    }
}