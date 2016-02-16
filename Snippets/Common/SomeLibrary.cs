using System.Threading.Tasks;

public static class SomeLibrary
{
    public static Task SomeAsyncMethod(params object[] data)
    {
        return Task.FromResult(0);
    }
    public static void SomeMethod(params object[] data)
    {
        //no-op
    }
}