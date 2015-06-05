using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class TypeScanner
{
    public static IEnumerable<Type> NestedTypes<T>(params Type[] extraTypes)
    {
        Type rootType = typeof(T);
        yield return rootType;
        foreach (var nestedType in rootType.GetNestedTypes(BindingFlags.NonPublic))
        {
            yield return nestedType;
        }
        foreach (var extraType in extraTypes)
        {
            yield return extraType;
        }

    }

    public static IEnumerable<Type> GetTypesToExclude<T>(params Type[] extraTypes)
    {
        Type rootType = typeof(T);

        var typesToInclude = NestedTypes<T>(extraTypes).ToList();

        return rootType.Assembly.GetAllTypes().Where(t => !typesToInclude.Contains(t)).ToList();
    }

    static IEnumerable<Type> GetAllTypes(this Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            yield return type;
            foreach (var nestedType in type.GetNestedTypes())
            {
                yield return nestedType;
            }
        }
    }
}