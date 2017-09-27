using System;
using System.Threading.Tasks;

class ChaosGenerator
{
    public bool IsFailing { get; set; }

    public Task Invoke()
    {
        if (IsFailing)
        {
            throw new Exception("Database is down");
        }
        return Task.CompletedTask;
    }
}