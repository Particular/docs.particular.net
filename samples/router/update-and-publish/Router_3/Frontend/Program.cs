using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

public class Program
{
    public static Task Main()
    {
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();

        return host.RunAsync();
    }
}