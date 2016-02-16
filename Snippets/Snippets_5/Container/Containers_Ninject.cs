namespace Snippets5.Container
{
    using Ninject;
    using NServiceBus;
    
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

        class MyService
        {
        }
    }
}