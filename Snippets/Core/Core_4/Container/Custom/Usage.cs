namespace Core4.Container.Custom
{
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;

    class Usage
    {
        Usage(Configure configure)
        {
            #region CustomContainerUsage

            configure.UsingContainer<MyContainer>();

            #endregion
        }
    }
}