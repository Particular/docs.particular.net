using Autofac;
using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region Autofac

        configure.AutofacBuilder();

        #endregion
    }

    void Existing(Configure configure)
    {
        #region Autofac_Existing

        var builder = new ContainerBuilder();
        builder.RegisterInstance(new MyService());
        var container = builder.Build();
        configure.AutofacBuilder(container);

        #endregion
    }

    class MyService
    {
    }
}