using System;
using System.Threading.Tasks;

public static class SomeLibrary
{
    public static Task SomeMethodAsync(Object message)
    {
        return Task.FromResult(0);
    }
    public static void SomeMethod(Object message)
    {
        //no-op
    }
}