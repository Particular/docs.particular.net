using System;
using System.Threading.Tasks;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Red.Router";

        var routerConfig = await RouterConfigurator.Prepare(ConnectionStrings.Red, "Red");

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Red);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Red);

        var router = Router.Create(routerConfig);

        await router.Start();

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop();
    }
}