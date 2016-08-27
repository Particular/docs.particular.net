namespace Core5.Container.Custom
{
    using NServiceBus;

    public class Usage
    {

        Usage(BusConfiguration busConfiguration)
        {
            #region CustomContainerUsage
            busConfiguration.UseContainer<MyContainerDefinition>();
            #endregion
        }

    }
}