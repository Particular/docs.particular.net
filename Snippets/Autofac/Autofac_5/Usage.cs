using Autofac;
using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region Autofac

        busConfiguration.UseContainer<AutofacBuilder>();

        #endregion
    }

    void Existing(BusConfiguration busConfiguration)
    {
        #region Autofac_Existing

        var builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        var container = builder.Build();
        busConfiguration.UseContainer<AutofacBuilder>(
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