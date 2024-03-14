namespace PlatformTools
{
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main()
        {
            await Particular.PlatformLauncher.Launch();
        }
    }
}
