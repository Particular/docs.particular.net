using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.ObjectBuilder.Common;
using NServiceBus.ObjectBuilder.Common.Config;

public class ContainerCustom
{
    // startcode CustomContainers
    public void CustomContainerUsage()
    {
        //Call `Configure.UsingContainer<T>()` in your configuration
        Configure.With()
            .UsingContainer<MyCustomObjectBuilder>();
    }

    //Create a class that implements `IContainer`
    public class MyCustomObjectBuilder : IContainer
    {
        // endcode
        public void Dispose()
        {
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
        }

        public void Configure<T>(Func<T> component, DependencyLifecycle dependencyLifecycle)
        {
        }

        public void ConfigureProperty(Type component, string property, object value)
        {
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
        }

        public bool HasComponent(Type componentType)
        {
            throw new NotImplementedException();
        }

        public void Release(object instance)
        {
        }
    }
}