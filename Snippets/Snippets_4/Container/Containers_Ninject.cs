namespace Snippets4.Container
{
    using Ninject;
    using NServiceBus;

    public class Containers_Ninject
    {
        public void Simple()
        {
            Configure configure = Configure.With();
            #region Ninject

            configure.NinjectBuilder();

            #endregion
        }

        public void Existing()
        {
            
            Configure configure = Configure.With();
            #region Ninject_Existing

            StandardKernel kernel = new StandardKernel();
            kernel.Bind<MyService>().ToConstant(new MyService());
            configure.NinjectBuilder(kernel);
            #endregion
        }

    }
}