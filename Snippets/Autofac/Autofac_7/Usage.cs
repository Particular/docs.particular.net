using Autofac;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Autofac

        endpointConfiguration.UseContainer<AutofacBuilder>();

        #endregion
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
        #region Autofac_Existing

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