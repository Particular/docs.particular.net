namespace Core7.Container.Custom
{
    using NServiceBus;

    public class Usage
    {

        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region CustomContainerUsage
            endpointConfiguration.UseContainer<MyContainerDefinition>();
            #endregion
        }
    }
}