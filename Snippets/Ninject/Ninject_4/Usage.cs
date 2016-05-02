using Ninject;
using NServiceBus;

class Usage
{
    Usage(Configure configure)
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

    class MyService
    {
    }
}