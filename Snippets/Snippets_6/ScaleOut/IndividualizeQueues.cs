namespace Snippets6.ScaleOut
{
    using NServiceBus;

    class IndividualizeQueues
    {
        public void Simple()
        {
            EndpointConfiguration busConfiguration = new EndpointConfiguration();
            
            #region UniqueQueuePerEndpointInstanceDiscriminator

            busConfiguration.ScaleOut()
                .InstanceDiscriminator("MyInstanceID");

            #endregion
        }
    }
}
