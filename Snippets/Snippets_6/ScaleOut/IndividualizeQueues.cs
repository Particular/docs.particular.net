namespace Snippets6.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        public void Simple()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            
            #region UniqueQueuePerEndpointInstanceDiscriminator

            endpointConfiguration.ScaleOut()
                .InstanceDiscriminator("MyInstanceID");

            #endregion
        }
    }
}
