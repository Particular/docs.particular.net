namespace Helper;

public class ConsoleHelper
{
#pragma warning disable PS0018
    public static Task WriteMessageProcessed(DateTime sentTime)
#pragma warning restore PS0018
    {
        lock (Console.Out)
        {
            if (DateTime.UtcNow - sentTime >= TimeSpan.FromSeconds(10))
            {
                Console.Write("\x1b[92m■\x1b[0m");
            }
            else
            {
                Console.Write("·");
            }
        }

        return Task.CompletedTask;
    }
}