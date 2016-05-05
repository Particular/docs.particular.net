namespace Core6.Container.Custom
{
    using NServiceBus;
    using NServiceBus.Container;
    using NServiceBus.ObjectBuilder.Common;
    using NServiceBus.Settings;

    public class Usage
    {
        #region CustomContainers

        public void CustomContainerExtensionUsage(EndpointConfiguration endpointConfiguration)
        {
            //Register the container in the configuration with '.UseContainer<T>()'
            endpointConfiguration.UseContainer<MyContainer>();
        }

        // Create a class that implements 'ContainerDefinition' and returns the 'IContainer' implementation.
        public class MyContainer : ContainerDefinition
        {
            public override IContainer CreateContainer(ReadOnlySettings settings)
            {
                //Create a class that implements 'IContainer'
                return new MyObjectBuilder();
            }
        }

        #endregion
    }
}