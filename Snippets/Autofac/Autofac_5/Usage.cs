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

        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        IContainer container = builder.Build();
        busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

        #endregion
    }

    class MyService
    {
    }
}