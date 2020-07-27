namespace Core7.Container.Custom
{
    using NServiceBus.Container;
    using NServiceBus.ObjectBuilder.Common;
    using NServiceBus.Settings;

    #region CustomContainerDefinition

    public class MyContainerDefinition :
        ContainerDefinition
    {
        public override IContainer CreateContainer(ReadOnlySettings settings)
        {
            return new MyContainer();
        }
    }

    #endregion
}