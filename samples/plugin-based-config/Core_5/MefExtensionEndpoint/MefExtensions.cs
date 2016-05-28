using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
#region MefExtensions
public static class MefExtensions
{
    public static void ExecuteExports<T>(this CompositionContainer container, Action<T> action)
    {
        foreach (var export in container.GetAllExports<T>())
        {
            action(export);
        }
    }
    static IEnumerable<T> GetAllExports<T>(this CompositionContainer container)
    {
        return container.GetExports<T>().Select(x => x.Value);
    }
}
#endregion