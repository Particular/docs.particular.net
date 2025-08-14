namespace Common;

using System.Threading.Tasks;

public static class SomeLibrary
{
    public static Task SomeAsyncMethod(params object[] data)
    {
        return Task.CompletedTask;
    }
    public static Task AnotherAsyncMethod(params object[] data)
    {
        return Task.CompletedTask;
    }
    public static void SomeMethod(params object[] data)
    {
        // no-op
    }
}