using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System.Linq;

class DIShims
{
    void Old()
    {
        IServiceCollection services = new ServiceCollection();

#pragma warning disable CS0618 // Type or member is obsolete
        #region core-8to9-di-shims-old
        // 1
        services.ConfigureComponent(typeof(MyDependency), DependencyLifecycle.SingleInstance);
        // 2
        services.ConfigureComponent<MyDependency>(DependencyLifecycle.SingleInstance);
        // 3
        services.ConfigureComponent<MyDependency>(DependencyLifecycle.InstancePerUnitOfWork);
        // 4
        services.ConfigureComponent<MyDependency>(DependencyLifecycle.InstancePerCall);
        // 5
        services.RegisterSingleton(new MyDependency());
        // 6
        if(services.HasComponent<MyDependency>())
        {
            // do something
        }
        #endregion
#pragma warning restore CS0618 // Type or member is obsolete

        #region core-8to9-di-shims-new
        // 1
        services.Add(new ServiceDescriptor(typeof(MyDependency), typeof(MyDependency), ServiceLifetime.Singleton));
        // 2
        services.AddSingleton<MyDependency>();
        // 3
        services.AddScoped<MyDependency>();
        // 4
        services.AddTransient<MyDependency>();
        // 5
        services.AddSingleton(new MyDependency());
        // 6
        if (services.AsEnumerable().Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(MyDependency)))
        {
            // do something
        }
        #endregion
    }

    class MyDependency { }
}