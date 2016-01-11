using System;
using System.Collections.Generic;
using System.Reflection;

public static class TypeScanner
{
    public static IEnumerable<Type> NestedTypes<T>(params Type[] extraTypes)
    {
        Type rootType = typeof(T);
        yield return rootType;
        foreach (Type nestedType in rootType.GetNestedTypes(BindingFlags.NonPublic))
        {
            yield return nestedType;
        }
        foreach (Type extraType in extraTypes)
        {
            yield return extraType;
        }

    }
}