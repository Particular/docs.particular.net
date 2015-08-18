namespace Snippets4.Container
{
    using Ninject;
    using NServiceBus;

    public class Containers_Ninject
    {
        public void Simple()
        {
            #region Ninject

            Configure configure = Configure.With();
            configure.NinjectBuilder();

            #endregion
        }

        public void Existing()
        {
            
            #region Ninject_Existing

            Configure configure = Configure.With();
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<MyService>().ToConstant(new MyService());
            configure.NinjectBuilder(kernel);
            #endregion
        }

    }
}