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
            foreach (Type type in assembly.GetInterfaceTypes<T>())
            {
                T instance = (T) Activator.CreateInstance(type);
                action(instance);
            }
        }
    }

    static IEnumerable<Type> GetInterfaceTypes<TInterface>(this Assembly assembly)
    {
        return from type in assembly.GetTypes()
               where typeof(TInterface).IsAssignableFrom(type)
               select type;
    }
}
#endregion