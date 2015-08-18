namespace Snippets3.Container
{
    using Ninject;
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;
    using NServiceBus.ObjectBuilder.Ninject;

    public class Containers_Ninject
    {
        public void Simple()
        {
            #region Ninject

            Configure.With()
                .UsingContainer<NinjectObjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            IKernel ninjectKernel = null;

            #region Ninject_Existing

            Configure.With()
                .UsingContainer(new NinjectObjectBuilder(ninjectKernel));

            #endregion
        }

    }
}