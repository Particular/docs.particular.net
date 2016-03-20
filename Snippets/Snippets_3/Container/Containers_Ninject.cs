namespace Snippets3.Container
{
    using Ninject;
    using NServiceBus;

    class Containers_Ninject
    {
        public void Simple(Configure configure)
        {
            #region Ninject

            configure.NinjectBuilder();

            #endregion
        }

        void Existing(Configure configure)
        {
            #region Ninject_Existing

            StandardKernel kernel = new StandardKernel();
            kernel.Bind<MyService>().ToConstant(new MyService());
            configure.NinjectBuilder(kernel);

            #endregion
        }

    }
}