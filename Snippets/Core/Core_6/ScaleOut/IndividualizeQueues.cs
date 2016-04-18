namespace Core6.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        IndividualizeQueues(EndpointConfiguration endpointConfiguration)
        {
            #region UniqueQueuePerEndpointInstanceDiscriminator

            endpointConfiguration.ScaleOut()
                .InstanceDiscriminator("MyInstanceID");

            #endregion
        }
    }
}
