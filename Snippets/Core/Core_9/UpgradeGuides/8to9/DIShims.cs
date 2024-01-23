using Microsoft.Extensions.DependencyInjection;
using System.Linq;

class DIShims
{
    void Configuration()
    {
        IServiceCollection services = new ServiceCollection();

        #region core-8to9-di-shims
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