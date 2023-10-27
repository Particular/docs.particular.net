namespace PlatformTools
{
    class Program
    {
        static async Task Main()
        {
            await Particular.PlatformLauncher.Launch()
                .ConfigureAwait(false);
        }
    }
}
