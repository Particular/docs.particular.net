namespace Core3.Container
{
    using NServiceBus;

    class Usage
    {
        void InstancePerCall(Configure configure)
        {
            #region InstancePerCall

            var components = configure.Configurer;
            components.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);

            #endregion
        }

        void DelegateInstancePerCall(Configure configure)
        {
            #region DelegateInstancePerCall

            var components = configure.Configurer;
            components.ConfigureComponent(
                componentFactory: () =>
                {
                    return new MyService();
                },
                dependencyLifecycle: DependencyLifecycle.InstancePerCall);

            #endregion
        }

        void InstancePerUnitOfWork(Configure configure)
        {
            #region InstancePerUnitOfWork

            var components = configure.Configurer;
            components.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerUnitOfWork);

            #endregion
        }

        void DelegateInstancePerUnitOfWork(Configure configure)
        {
            #region DelegateInstancePerUnitOfWork

            var components = configure.Configurer;
            components.ConfigureComponent(
                componentFactory: () =>
                {
                    return new MyService();
                },
                dependencyLifecycle: DependencyLifecycle.InstancePerUnitOfWork);

            #endregion
        }

        void SingleInstance(Configure configure)
        {
            #region SingleInstance

            var components = configure.Configurer;
            components.ConfigureComponent<MyService>(DependencyLifecycle.SingleInstance);

            #endregion
        }

        void DelegateSingleInstance(Configure configure)
        {
            #region DelegateSingleInstance

            var components = configure.Configurer;
            components.ConfigureComponent(
                componentFactory: () =>
                {
                    return new MyService();
                },
                dependencyLifecycle: DependencyLifecycle.SingleInstance);

            #endregion
        }

        void RegisterSingleton(Configure configure)
        {
            #region RegisterSingleton

            var components = configure.Configurer;
            components.RegisterSingleton<MyService>(new MyService());

            #endregion
        }

    }
}