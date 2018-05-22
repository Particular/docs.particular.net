using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SampleWeb
{
    class Program
    {
        public static async Task Main()
        {
            await Installation.Run().ConfigureAwait(false);
            var builder = WebHost.CreateDefaultBuilder();
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.UseStartup<Startup>();
            var webHost = builder.Build();
            webHost.Run();
        }
    }
}