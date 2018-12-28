using System;
using System.Threading.Tasks;
using NServiceBus.Router;

class Program
{
    static async Task Main()
    {
        Console.Title = "Green.Router";

        var routerConfig = await RouterConfigurator.Prepare(ConnectionStrings.Green, "Green").ConfigureAwait(false);

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Green);
        SqlHelper.CreateReceivedMessagesTable(ConnectionStrings.Green);

        var router = Router.Create(routerConfig);

        await router.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await router.Stop().ConfigureAwait(false);
    }


}