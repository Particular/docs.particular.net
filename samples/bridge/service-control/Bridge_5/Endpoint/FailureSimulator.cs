using System;
using System.Threading.Tasks;

public static class FailureSimulator
{
    public static bool Enabled { get; set; }

    public static Task Invoke()
    {
        if (Enabled)
        {
            throw new Exception("Database is down");
        }
        return Task.CompletedTask;
    }
}