namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.Features;

    class IConfigureComponentsNotInjected
    {
        public IConfigureComponentsNotInjected()
        {
            FeatureConfigurationContext context = null;

            #region 5to6-IConfigureComponentsNotInjected
            context.Container.ConfigureComponent(b => new MyDependency(context.Container), DependencyLifecycle.InstancePerCall);
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