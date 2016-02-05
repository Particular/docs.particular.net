namespace Snippets5.Container
{
    using Ninject;
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Ninject;

    public class Containers_Ninject
    {
        public void Simple()
        {
            #region Ninject

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<NinjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region Ninject_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<MyService>().ToConstant(new MyService());
            busConfiguration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(kernel));

            #endregion
        }

        public void UseUnitOfWorkScope()
        {
            #region NinjectUnitOfWork [4.0,6.0]

            StandardKernel kernel = new StandardKernel();

            kernel.Bind<MyService>().ToSelf().InUnitOfWorkScope();

            #endregion
        }

        public void UseConditionalBinding()
        {
            #region NinjectConditionalBindings [4.0,6.0]

            StandardKernel kernel = new StandardKernel();

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
}