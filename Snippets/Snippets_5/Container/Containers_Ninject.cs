namespace Snippets5.Container
{
    using Ninject;
    using NServiceBus;
    
    public class Containers_Ninject
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Ninject

            busConfiguration.UseContainer<NinjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Ninject_Existing

            StandardKernel kernel = new StandardKernel();
            kernel.Bind<MyService>().ToConstant(new MyService());
            busConfiguration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(kernel));

            #endregion
        }

        class MyService
        {
        }
    }
}