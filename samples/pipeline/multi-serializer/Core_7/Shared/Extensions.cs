using System;
using System.Linq;

static class Extensions
{
    public static bool ContainsAttribute<T>(this Type type)
    {
        return type.GetCustomAttributes(typeof(T), true).Any();
    }
}