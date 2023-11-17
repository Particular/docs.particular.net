using System;
using System.Composition.Hosting;
using System.Threading.Tasks;

#region MefExtensions
public static class MefExtensions
{
    public static async Task ExecuteExports<T>(this CompositionHost compositionHost, Func<T, Task> action)
    {
        foreach (var export in compositionHost.GetExports<T>())
        {
            await action(export)
                .ConfigureAwait(false);
        }
    }
}
#endregion