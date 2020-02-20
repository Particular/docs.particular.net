using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Extensions.DependencyInjection_1
{
    class Configuration
    {
        void AccessServiceCollection(EndpointConfiguration endpointConfiguration)
        {
            #region settings-servicecollection

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
            containerSettings.ServiceCollection.AddSingleton<MyService>();

            #endregion
        }

        void CustomizeContainer(EndpointConfiguration endpointConfiguration)
        {
            #region settings-configurecontainer

            // use Autofac:
            var containerSettings = endpointConfiguration.UseContainer(new AutofacServiceProviderFactory());
            containerSettings.ConfigureContainer(containerBuilder =>
            {
                // access Autofac native APIs here:
                containerBuilder.RegisterType<MyService>().AsSelf().SingleInstance();
                containerBuilder.RegisterAssemblyModules(Assembly.GetCallingAssembly());
            });

            #endregion
        }

        class MyService
        {
        }
    }
}
