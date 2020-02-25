using Autofac;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region Autofac

        endpointConfiguration.UseContainer<AutofacBuilder>();

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete
    }

    class MyService
    {
    }

}