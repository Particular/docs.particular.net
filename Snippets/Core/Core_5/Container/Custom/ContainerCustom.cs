namespace Core5.Container.Custom
{
    using NServiceBus;
    using NServiceBus.Container;
    using NServiceBus.ObjectBuilder.Common;
    using NServiceBus.Settings;

    public class Usage
    {
        #region CustomContainers

        public void CustomContainerExtensionUsage()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<MyContainer>();
        }

        // Create a class that implements 'ContainerDefinition' and returns the 'IContainer' implementation.
        public class MyContainer :
            ContainerDefinition
        {
            public override IContainer CreateContainer(ReadOnlySettings settings)
            {
                // Create a class that implements 'IContainer'
                return new MyObjectBuilder();
            }
        }

        #endregion
    }
}