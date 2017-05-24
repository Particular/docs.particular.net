namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.Features;

    class IConfigureComponentsNotInjected
    {
        public IConfigureComponentsNotInjected()
        {
            FeatureConfigurationContext context = null;

            #region 5to6-IConfigureComponentsNotInjected

            var container = context.Container;
            container.ConfigureComponent(
                componentFactory: builder => new MyDependency(container),
                dependencyLifecycle: DependencyLifecycle.InstancePerCall);
            #endregion
        }

        class MyDependency
        {
            // ReSharper disable once UnusedParameter.Local
            public MyDependency(object container)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}