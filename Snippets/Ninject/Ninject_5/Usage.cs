namespace Snippets5.Container
{
    using Ninject;
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region Ninject

            busConfiguration.UseContainer<NinjectBuilder>();

            #endregion
        }

        void Existing(BusConfiguration busConfiguration)
        {
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