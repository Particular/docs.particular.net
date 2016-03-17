namespace Snippets4.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    public class Containers_Unity
    {
        public void Simple()
        {
            Configure configure = Configure.With();
            #region Unity

            configure.UnityBuilder();

            #endregion
        }

        public void Existing()
        {


            Configure configure = Configure.With();
            #region Unity_Existing
            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            configure.UnityBuilder(container);

            #endregion
        }

    }
}