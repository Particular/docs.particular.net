namespace Core3.Container.Custom
{
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;

    class Usage
    {
        Usage(Configure configure)
        {
            #region CustomContainerUsage

            // Create a class that implements 'IContainer'
            configure.UsingContainer<MyContainer>();

            #endregion
        }
    }
}