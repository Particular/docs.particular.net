using System;
using System.Threading.Tasks;
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedTypeParameter

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