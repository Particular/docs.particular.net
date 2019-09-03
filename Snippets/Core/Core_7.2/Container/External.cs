namespace Core7.Container.Custom
{
    using NServiceBus;

    public class External
    {
        void Usage(EndpointConfiguration endpointConfiguration)
        {
            #region CustomContainerUsage
            endpointConfiguration.UseContainer<MyContainerDefinition>();
            #endregion
        }
    }
}