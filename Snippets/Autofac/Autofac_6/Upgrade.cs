using Autofac;
using NServiceBus;

class Upgrade
{
    Upgrade(EndpointConfiguration endpointConfiguration)
    {
        #region 5-to-6-autofac

        endpointConfiguration.UseContainer<AutofacBuilder>();

        #endregion
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
        #region 5-to-6-autofac-existing

        var builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        var container = builder.Build();
        endpointConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });

        #endregion
    }

    class MyService
    {
    }

}