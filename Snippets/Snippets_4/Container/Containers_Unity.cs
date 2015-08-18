namespace Snippets4.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    public class Containers_Unity
    {
        public void Simple()
        {
            #region Unity

            Configure configure = Configure.With();
            configure.UnityBuilder();

            #endregion
        }

        public void Existing()
        {

            #region Unity_Existing

            Configure configure = Configure.With();
            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            configure.UnityBuilder(container);

            #endregion
        }

    }
}