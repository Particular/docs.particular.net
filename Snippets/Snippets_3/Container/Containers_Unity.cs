namespace Snippets3.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;
    using NServiceBus.ObjectBuilder.Unity;

    public class Containers_Unity
    {
        public void Simple()
        {
            #region Unity

            Configure.With()
                .UsingContainer<UnityObjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            UnityContainer unityContainer = null;

            #region Unity_Existing

            Configure.With()
                .UsingContainer(new UnityObjectBuilder(unityContainer));

            #endregion
        }

    }
}