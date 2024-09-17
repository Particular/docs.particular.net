using System;
using System.Runtime.InteropServices;
using System.Threading;

public class ConoleHelper
{
    const int STD_INPUT_HANDLE = -10;

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

    public static ConsoleKeyInfo ReadKeyAsync(CancellationToken cancellationToken)
    {
        using var reg = cancellationToken.Register(() =>
        {
            var handle = GetStdHandle(STD_INPUT_HANDLE);
            CancelIoEx(handle, IntPtr.Zero);
        });
        try
        {
            var key = Console.ReadKey();
            reg.Unregister();
            return key;
        }
        catch (InvalidOperationException)
        {
            // caused by CancelIoEx
            throw new OperationCanceledException(cancellationToken);
        }
    }
}