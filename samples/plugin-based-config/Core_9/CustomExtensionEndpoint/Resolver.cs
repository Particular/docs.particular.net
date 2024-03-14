using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

#region Resolver
public static class Resolver
{
    static readonly List<Assembly> assemblies;

    static Resolver()
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);
        var targetFramework = currentDirectory.Name;
        var configuration = currentDirectory.Parent.Name;

        var solutionDirectory = currentDirectory.Parent.Parent.Parent.Parent.FullName;
        var extensionDirectory = Path.Combine(solutionDirectory, "CustomExtensions", "bin", configuration, targetFramework);

        assemblies = Directory.GetFiles(extensionDirectory, "*.dll")
            .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
            .Where(ReferencesShared)
            .ToList();
    }

    static bool ReferencesShared(Assembly assembly)
    {
        var sharedAssembly = typeof(ICustomizeConfiguration).Assembly.GetName().Name;
        return assembly.GetReferencedAssemblies()
            .Any(name => name.Name == sharedAssembly);
    }

    public static async Task Execute<T>(Func<T, Task> action)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetImplementationTypes<T>())
            {
                var instance = (T)Activator.CreateInstance(type);
                await action(instance);
            }
        }
    }

    static IEnumerable<Type> GetImplementationTypes<TInterface>(this Assembly assembly)
    {
        return assembly.GetTypes().Where(IsConcreteClass<TInterface>);
    }

    static bool IsConcreteClass<TInterface>(Type type)
    {
        return typeof(TInterface).IsAssignableFrom(type) &&
               !type.IsAbstract &&
               !type.ContainsGenericParameters &&
               type.IsClass;
    }
}
#endregion