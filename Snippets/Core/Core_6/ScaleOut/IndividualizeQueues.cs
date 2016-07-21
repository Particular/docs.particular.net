namespace Core6.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        IndividualizeQueues(EndpointConfiguration endpointConfiguration)
        {
            #region UniqueQueuePerEndpointInstanceDiscriminator

            var scaleOut = endpointConfiguration.ScaleOut();
            scaleOut.InstanceDiscriminator("MyInstanceID");

            #endregion
        }
    }
}
