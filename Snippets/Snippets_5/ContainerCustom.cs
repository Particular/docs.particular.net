using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Container;
using NServiceBus.ObjectBuilder.Common;
using NServiceBus.Settings;

public class ContainerCustom
{
    // startcode CustomContainers

    public void CustomContainerExtensionUsage()
    {
        var configuration = new BusConfiguration();
        //Register the container in the configuration with '.UseContainer<T>()'
        configuration.UseContainer<MyContainer>();
    }
    // Create a class that implements 'ContainerDefinition' and returns your 'IContainer' implementation.
    public class MyContainer : ContainerDefinition
    {
        public override IContainer CreateContainer(ReadOnlySettings settings)
        {
            return new MyObjectBuilder();
        }
    }
    //Create a class that implements 'IContainer'
    public class MyObjectBuilder : IContainer
    {
    // endcode 

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object Build(Type typeToBuild)
        {
            throw new NotImplementedException();
        }

        public IContainer BuildChildContainer()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> BuildAll(Type typeToBuild)
        {
            throw new NotImplementedException();
        }

        public void Configure(Type component, DependencyLifecycle dependencyLifecycle)
        {
            throw new NotImplementedException();
        }

        public void Configure<T>(Func<T> component, DependencyLifecycle dependencyLifecycle)
        {
            throw new NotImplementedException();
        }

        public void ConfigureProperty(Type component, string property, object value)
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            throw new NotImplementedException();
        }

        public bool HasComponent(Type componentType)
        {
            throw new NotImplementedException();
        }

        public void Release(object instance)
        {
            throw new NotImplementedException();
        }
    }
}
