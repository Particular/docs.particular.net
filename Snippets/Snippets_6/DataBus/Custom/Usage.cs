namespace Snippets6.DataBus.Custom
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region PluginCustomDataBus

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseDataBus(typeof(CustomDataBus));

            #endregion
        }
    }
}
