using Microsoft.AspNetCore.Hosting;
using System.IO;

static class Program
{
    static void Main()
    {
        var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

        host.Run();
    }
}
