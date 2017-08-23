using Ninject;
using NServiceBus;
using NServiceBus.ObjectBuilder.Ninject;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Ninject

        endpointConfiguration.UseContainer<NinjectBuilder>();

        #endregion
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
        #region Ninject_Existing

        var kernel = new StandardKernel();
        kernel.Bind<MyService>()
            .ToConstant(new MyService());
        endpointConfiguration.UseContainer<NinjectBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingKernel(kernel);
            });

        #endregion
    }

    void UseUnitOfWorkScope()
    {
        #region NinjectUnitOfWork

        var kernel = new StandardKernel();

        kernel.Bind<MyService>()
            .ToSelf()
            .InUnitOfWorkScope();

        #endregion
    }

    void UseConditionalBinding()
    {
        #region NinjectConditionalBindings

        var kernel = new StandardKernel();

        // always create a new instance when not processing a message
        kernel.Bind<MyService>().ToSelf()
            .WhenNotInUnitOfWork()
            .InTransientScope();

        // always use the same instance when processing messages
        kernel.Bind<MyService>().ToSelf()
            .WhenInUnitOfWork()
            .InSingletonScope();

        #endregion
    }

    class MyService
    {
    }
}