using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Extensions.DependencyInjection_1
{
    class Usage
    {
        void ConfigureServiceCollection(EndpointConfiguration endpointConfiguration)
        {
            #region usecontainer-servicecollection

            endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());

            #endregion
        }

        void ConfigureThirdPartyContainer(EndpointConfiguration endpointConfiguration)
        {
            #region usecontainer-thirdparty

            endpointConfiguration.UseContainer(new AutofacServiceProviderFactory());

            #endregion
        }
    }
}