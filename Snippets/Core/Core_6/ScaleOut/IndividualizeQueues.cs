namespace Core6.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        IndividualizeQueues(EndpointConfiguration endpointConfiguration)
        {
            #region UniqueQueuePerEndpointInstanceDiscriminator

            endpointConfiguration.MakeInstanceUniquelyAddressable("MyInstanceID");

            #endregion
        }
    }
}
