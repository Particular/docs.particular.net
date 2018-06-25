namespace Core6.Container
{
    using NServiceBus;

    class Usage
    {
        void InstancePerCall(EndpointConfiguration endpointConfiguration)
        {
            #region InstancePerCall

            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);
                });

            #endregion
        }

        void DelegateInstancePerCall(EndpointConfiguration endpointConfiguration)
        {
            #region DelegateInstancePerCall

            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent(
                        componentFactory: () =>
                        {
                            return new MyService();
                        },
                        dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                });

            #endregion
        }

        void InstancePerUnitOfWork(EndpointConfiguration endpointConfiguration)
        {
            #region InstancePerUnitOfWork

            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerUnitOfWork);
                });

            #endregion
        }

        void DelegateInstancePerUnitOfWork(EndpointConfiguration endpointConfiguration)
        {
            #region DelegateInstancePerUnitOfWork

            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent(
                        componentFactory: () =>
                        {
                            return new MyService();
                        },
                        dependencyLifecycle: DependencyLifecycle.InstancePerUnitOfWork);
                });

            #endregion
        }

        void SingleInstance(EndpointConfiguration endpointConfiguration)
        {
            #region SingleInstance

            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent<MyService>(DependencyLifecycle.SingleInstance);
                });

            #endregion
        }

        void DelegateSingleInstance(EndpointConfiguration endpointConfiguration)
        {
            #region DelegateSingleInstance

            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent(
                        componentFactory: () =>
                        {
                            return new MyService();
                        },
                        dependencyLifecycle: DependencyLifecycle.SingleInstance);
                });

            #endregion
        }

        void RegisterSingleton(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterSingleton

            endpointConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.RegisterSingleton(new MyService());
                });

            #endregion
        }

    }
}