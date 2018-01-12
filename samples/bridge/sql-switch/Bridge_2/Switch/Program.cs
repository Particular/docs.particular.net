using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;

class Program
{
    static async Task Main()
    {
        Console.Title = "Switch";

        #region SwitchConfig

        var switchConfig = new SwitchConfiguration();
        switchConfig.AddPort<SqlServerTransport>("Blue", t =>
        {
            t.ConnectionString(ConnectionStrings.Blue);
        }).UseSubscriptionPersistece<InMemoryPersistence>(p => { });

        switchConfig.AddPort<SqlServerTransport>("Red", t =>
        {
            t.ConnectionString(ConnectionStrings.Red);
        }).UseSubscriptionPersistece<InMemoryPersistence>(p => { });

        switchConfig.AddPort<SqlServerTransport>("Green", t =>
        {
            t.ConnectionString(ConnectionStrings.Green);
        }).UseSubscriptionPersistece<InMemoryPersistence>(p => { });

        switchConfig.AutoCreateQueues();

        #endregion

        #region SwitchForwarding

        switchConfig.PortTable["Client"] = "Blue";
        switchConfig.PortTable["Sales"] = "Red";
        switchConfig.PortTable["Shipping"] = "Red";
        switchConfig.PortTable["Billing"] = "Green";

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Blue);
        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Red);
        SqlHelper.EnsureDatabaseExists(ConnectionStrings.Green);

        var @switch = Switch.Create(switchConfig);

        await @switch.Start().ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await @switch.Stop().ConfigureAwait(false);
    }


}