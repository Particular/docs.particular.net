namespace Snippets5.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    public class Containers_Unity
    {
        public void Simple()
        {
            #region Unity

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<UnityBuilder>();

            #endregion
        }

        public void Existing()
        {
            UnityContainer unityContainer = null;

            #region Unity_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(unityContainer));

            #endregion
        }

    }
}