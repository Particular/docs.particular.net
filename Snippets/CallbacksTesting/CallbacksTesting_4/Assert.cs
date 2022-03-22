using System;
using System.Threading.Tasks;

static class Assert
{
    public static void AreEqual(object expected, object result)
    {
    }

    public static void ThrowsAsync<TException>(Func<Task> @delegate)
        where TException : Exception
    {
    }
}