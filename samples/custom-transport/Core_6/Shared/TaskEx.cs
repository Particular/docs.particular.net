using System.Threading.Tasks;

public static class TaskEx
{
    // Used to explicitly suppress the compiler warning about 
    // using the returned value from async operations
    public static void Ignore(this Task task)
    {
    }
    public static readonly Task CompletedTask = Task.FromResult(0);
}