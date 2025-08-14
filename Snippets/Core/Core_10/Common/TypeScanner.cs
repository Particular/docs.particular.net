namespace Common;

using System;
using System.Collections.Generic;
using System.Reflection;

public static class TypeScanner
{
    public static IEnumerable<Type> NestedTypes<T>(params Type[] extraTypes)
    {
        var rootType = typeof(T);
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
}