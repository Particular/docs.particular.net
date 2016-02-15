namespace Snippets6.Transports.Throughput
{
    using System.Configuration;
    using NServiceBus;

    class ConcurrencyConfiguration
    {
        void ConfigureFromCode()
        {
            #region TuningFromCode
            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.LimitMessageProcessingConcurrencyTo(5);
            #endregion
        }

        void ConfigureFromAppConfig()
        {
            #region TuningFromAppConfig
            EndpointConfiguration configuration = new EndpointConfiguration();

            // add your custom key to the appSettings section like this:
            // <appSettings>
            //    <add key="MaxConcurrency" value="5"/>
            // </appSettings>
            configuration.LimitMessageProcessingConcurrencyTo(int.Parse(ConfigurationManager.AppSettings["MaxConcurrency"]));
            #endregion
        }
    }
}