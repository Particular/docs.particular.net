namespace Core5.Container
{
    using NServiceBus;

    class Usage
    {
        void InstancePerCall(BusConfiguration busConfiguration)
        {
            #region InstancePerCall

            busConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);
                });

            #endregion
        }

        void DelegateInstancePerCall(BusConfiguration busConfiguration)
        {
            #region DelegateInstancePerCall

            busConfiguration.RegisterComponents(
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

        void InstancePerUnitOfWork(BusConfiguration busConfiguration)
        {
            #region InstancePerUnitOfWork

            busConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerUnitOfWork);
                });

            #endregion
        }

        void DelegateInstancePerUnitOfWork(BusConfiguration busConfiguration)
        {
            #region DelegateInstancePerUnitOfWork

            busConfiguration.RegisterComponents(
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

        void SingleInstance(BusConfiguration busConfiguration)
        {
            #region SingleInstance

            busConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.ConfigureComponent<MyService>(DependencyLifecycle.SingleInstance);
                });

            #endregion
        }

        void DelegateSingleInstance(BusConfiguration busConfiguration)
        {
            #region DelegateSingleInstance

            busConfiguration.RegisterComponents(
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

        void RegisterSingleton(BusConfiguration busConfiguration)
        {
            #region RegisterSingleton

            busConfiguration.RegisterComponents(
                registration: configureComponents =>
                {
                    configureComponents.RegisterSingleton(new MyService());
                });

            #endregion
        }

    }
}