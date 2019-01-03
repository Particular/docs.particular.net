using System;
using System.Threading.Tasks;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Blue.Router";

        var routerConfig = await RouterConfigurator.Prepare(ConnectionStrings.Blue, "Blue").ConfigureAwait(false);

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Blue);

        var router = Router.Create(routerConfig);

        await router.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop().ConfigureAwait(false);
    }
}