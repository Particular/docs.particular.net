namespace LearningTransport
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region LearningTransport

            endpointConfiguration.UseTransport<LearningTransport>();

            #endregion
        }
    }
}