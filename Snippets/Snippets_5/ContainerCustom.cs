using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Container;
using NServiceBus.ObjectBuilder.Common;
using NServiceBus.Settings;

public class ContainerCustom
{
    #region CustomContainersV5

    public void CustomContainerExtensionUsage()
    {
        Configure.With(b => b.UseContainer<MyContainer>());
    }

    public class MyContainer : ContainerDefinition
    {
        public override IContainer CreateContainer(ReadOnlySettings settings)
        {
            return new MyObjectBuilder();
        }
    }

    #endregion

}

public class MyObjectBuilder : IContainer
{
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