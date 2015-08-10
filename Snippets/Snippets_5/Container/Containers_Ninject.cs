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
            IKernel ninjectKernel = null;

            #region Ninject_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(ninjectKernel));

            #endregion
        }

        public void UseUnitOfWorkScope()
        {
            #region NinjectUnitOfWork [4.0,5.0]

            var kernel = new StandardKernel();

            kernel.Bind<MyService>().ToSelf().InUnitOfWorkScope();

            #endregion
        }

        public void UseConditionalBinding()
        {
            #region NinjectConditionalBindings [4.0,5.0]

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
}