using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

#region Resolver
public static class Resolver
{
    static List<Assembly> assemblies;

    static Resolver()
    {
        var codebase = typeof(Resolver).Assembly.Location.Remove(0, 8);
        var currentDirectory = Path.GetDirectoryName(codebase);
        assemblies = Directory.GetFiles(currentDirectory, "*.dll")
            .Select(Assembly.LoadFrom)
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
                await action(instance)
                    .ConfigureAwait(false);
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