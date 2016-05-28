using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;

#region MefExtensions
public static class MefExtensions
{
    public static async Task ExecuteExports<T>(this CompositionContainer container, Func<T, Task> action)
    {
        foreach (var export in container.GetAllExports<T>())
        {
            await action(export)
                .ConfigureAwait(false);
        }
    }
    static IEnumerable<T> GetAllExports<T>(this CompositionContainer container)
    {
        return container.GetExports<T>().Select(x => x.Value);
    }
}
#endregion