namespace Snippets6.DataBus.Custom
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region PluginCustomDataBus

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseDataBus(typeof(CustomDataBus));

            #endregion
        }
    }
}
