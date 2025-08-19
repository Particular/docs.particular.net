using System;
using System.Threading;
using System.Threading.Tasks;

public class ConsoleHelper
{
    public static async Task<ConsoleKey> ReadKeyAsync(CancellationToken cancellationToken)
    {
        // if there is a key available, return it without waiting
        //  (or dispatching work to the thread queue)
        if (Console.KeyAvailable)
        {
            var read = Console.ReadKey(false);
            return read.Key;
        }

        // otherwise
        var result = await Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100, cancellationToken);
                if (Console.KeyAvailable)
                {
                    var read = Console.ReadKey(false);
                    return read.Key;
                }
            }
            cancellationToken.ThrowIfCancellationRequested();

            //We should not get here
            throw new OperationCanceledException(cancellationToken);
        }, cancellationToken);
        return result;
    }
}