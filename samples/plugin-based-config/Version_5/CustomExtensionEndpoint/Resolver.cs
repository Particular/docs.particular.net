using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
#region Resolver
public static class Resolver
{
    static List<Assembly> assemblies;

    static Resolver()
    {
        string codebase = typeof(Resolver).Assembly.CodeBase.Remove(0, 8);
        string currentDirectory = Path.GetDirectoryName(codebase);
        assemblies = Directory.GetFiles(currentDirectory, "*.dll")
            .Select(Assembly.LoadFrom)
            .Where(ReferecesShared)
            .ToList();
    }

    static bool ReferecesShared(Assembly assembly)
    {
        return assembly.GetReferencedAssemblies()
            .Any(name => name.Name == "Shared");
    }

    public static void Execute<T>(Action<T> action)
    {
        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetImplementationTypes<T>())
            {
                T instance = (T) Activator.CreateInstance(type);
                action(instance);
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