using System.Threading.Tasks;

public static class SomeLibrary
{
    public static Task SomeMethodAsync(object data)
    {
        return Task.FromResult(0);
    }
    public static void SomeMethod(object data)
    {
        //no-op
    }
}