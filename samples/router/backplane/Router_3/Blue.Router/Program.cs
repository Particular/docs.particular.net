using System;
using System.Threading.Tasks;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Blue.Router";

        var routerConfig = await RouterConfigurator.Prepare(ConnectionStrings.Blue, "Blue");

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Blue);

        var router = Router.Create(routerConfig);

        await router.Start();

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop();
    }
}