using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NServiceBus;

namespace ASBFunctions_6_0.TopologyOptions
{
    #region asb-function-topology-options
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.UseNServiceBus();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
    #endregion
}
