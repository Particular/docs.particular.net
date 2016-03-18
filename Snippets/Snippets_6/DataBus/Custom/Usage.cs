namespace Snippets6.DataBus.Custom
{
    using NServiceBus;

    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region PluginCustomDataBus
            endpointConfiguration.UseDataBus(typeof(CustomDataBus));

            #endregion
        }
    }
}
