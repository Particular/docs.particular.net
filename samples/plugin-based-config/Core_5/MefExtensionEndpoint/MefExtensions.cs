using System;
using System.Composition.Hosting;

#region MefExtensions
public static class MefExtensions
{
    public static void ExecuteExports<T>(this CompositionHost compositionHost, Action<T> action)
    {
        foreach (var export in compositionHost.GetExports<T>())
        {
            action(export);
        }
    }
}
#endregion