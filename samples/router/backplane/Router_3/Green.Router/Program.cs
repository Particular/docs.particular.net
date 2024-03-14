using System;
using System.Threading.Tasks;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Green.Router";

        var routerConfig = await RouterConfigurator.Prepare(ConnectionStrings.Green, "Green");

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Green);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Green);

        var router = Router.Create(routerConfig);

        await router.Start();

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop();
    }


}