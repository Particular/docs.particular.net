namespace NServiceBus.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.ObjectBuilder;

    class ServiceCollectionAdapter : IConfigureComponents
    {
        public ServiceCollectionAdapter(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        public void ConfigureComponent(Type concreteComponent, DependencyLifecycle dependencyLifecycle)
        {
            if (serviceCollection.Any(s => s.ServiceType == concreteComponent))
            {
                return;
            }

            serviceCollection.Add(new ServiceDescriptor(concreteComponent, concreteComponent, Map(dependencyLifecycle)));
            RegisterInterfaces(concreteComponent);
        }

        public void ConfigureComponent<T>(DependencyLifecycle dependencyLifecycle)
        {
            ConfigureComponent(typeof(T), dependencyLifecycle);
        }

        public void ConfigureComponent<T>(Func<T> componentFactory, DependencyLifecycle dependencyLifecycle)
        {
            var componentType = typeof(T);
            if (serviceCollection.Any(s => s.ServiceType == componentType))
            {
                return;
            }

            serviceCollection.Add(new ServiceDescriptor(componentType, p => componentFactory(), Map(dependencyLifecycle)));
            RegisterInterfaces(componentType);
        }

        public void ConfigureComponent<T>(Func<IBuilder, T> componentFactory, DependencyLifecycle dependencyLifecycle)
        {
            var componentType = typeof(T);
            if (serviceCollection.Any(s => s.ServiceType == componentType))
            {
                return;
            }

            serviceCollection.Add(new ServiceDescriptor(componentType, p => componentFactory(new ServiceProviderAdapter(p)), Map(dependencyLifecycle)));
            RegisterInterfaces(componentType);
        }

        public bool HasComponent<T>()
        {
            return HasComponent(typeof(T));
        }

        public bool HasComponent(Type componentType)
        {
            return serviceCollection.Any(sd => sd.ServiceType == componentType);
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            serviceCollection.AddSingleton(lookupType, instance);
        }

        public void RegisterSingleton<T>(T instance)
        {
            RegisterSingleton(typeof(T), instance);
        }

        void RegisterInterfaces(Type component)
        {
            var interfaces = component.GetInterfaces();
            foreach (var serviceType in interfaces)
            {
                // see https://andrewlock.net/how-to-register-a-service-with-multiple-interfaces-for-in-asp-net-core-di/
                serviceCollection.Add(new ServiceDescriptor(serviceType, sp => sp.GetService(component), ServiceLifetime.Transient));
            }
        }

        IServiceCollection serviceCollection;

        static ServiceLifetime Map(DependencyLifecycle lifetime)
        {
            switch (lifetime)
            {
                case DependencyLifecycle.SingleInstance: return ServiceLifetime.Singleton;
                case DependencyLifecycle.InstancePerCall: return ServiceLifetime.Transient;
                case DependencyLifecycle.InstancePerUnitOfWork: return ServiceLifetime.Scoped;
                default: throw new NotSupportedException();
            }
        }
    }
}