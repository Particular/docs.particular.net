using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ObjectBuilder;

public class ServiceProviderAdapter : IBuilder
{
    public ServiceProviderAdapter(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public object Build(Type typeToBuild)
    {
        return serviceProvider.GetService(typeToBuild) ?? throw new Exception($"Unable to build {typeToBuild.FullName}. Ensure the type has been registered correctly with the container.");
    }

    public T Build<T>()
    {
        return (T)Build(typeof(T));
    }

    public IEnumerable<T> BuildAll<T>()
    {
        return (IEnumerable<T>)BuildAll(typeof(T));
    }

    public IEnumerable<object> BuildAll(Type typeToBuild)
    {
        return serviceProvider.GetServices(typeToBuild);
    }

    public void BuildAndDispatch(Type typeToBuild, Action<object> action)
    {
        action(Build(typeToBuild));
    }

    public IBuilder CreateChildBuilder()
    {
        return new ChildScopeAdapter(serviceProvider.CreateScope());
    }

    public void Dispose()
    {
        //TODO: What do we do here?
    }

    public void Release(object instance)
    {
        //TODO: No-op?
    }

    IServiceProvider serviceProvider;

    class ChildScopeAdapter : IBuilder
    {
        public ChildScopeAdapter(IServiceScope serviceScope)
        {
            this.serviceScope = serviceScope;
        }

        public object Build(Type typeToBuild)
        {
            //TODO: no null check here?
            return serviceScope.ServiceProvider.GetService(typeToBuild);
        }

        public T Build<T>()
        {
            return (T)Build(typeof(T));
        }

        public IEnumerable<T> BuildAll<T>()
        {
            return (IEnumerable<T>)BuildAll(typeof(T));
        }

        public IEnumerable<object> BuildAll(Type typeToBuild)
        {
            return serviceScope.ServiceProvider.GetServices(typeToBuild);
        }

        public void BuildAndDispatch(Type typeToBuild, Action<object> action)
        {
            action(Build(typeToBuild));
        }

        public IBuilder CreateChildBuilder()
        {
            throw new InvalidOperationException();
        }

        public void Dispose()
        {
            serviceScope.Dispose();
        }

        public void Release(object instance)
        {
            //TODO: no-op?
        }

        IServiceScope serviceScope;
    }
}