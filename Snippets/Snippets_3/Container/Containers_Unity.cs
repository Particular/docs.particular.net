namespace Snippets3.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    class Containers_Unity
    {
        void Simple(Configure configure)
        {
            #region Unity

            configure.UnityBuilder();

            #endregion
        }

        void Existing(Configure configure)
        {
            #region Unity_Existing

            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            configure.UnityBuilder(container);

            #endregion
        }

    }
}