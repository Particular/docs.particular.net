using System;
using System.Collections.Generic;
using System.Reflection;

static class TypeScanner
{
    public static IEnumerable<Type> TypesFor<T>()
    {
        Type rootType = typeof(T);
        yield return rootType;
        foreach (var nestedType in rootType.GetNestedTypes(BindingFlags.NonPublic))
        {
            yield return nestedType;
        }
        yield return typeof(ConfigErrorQueue);

    }
}