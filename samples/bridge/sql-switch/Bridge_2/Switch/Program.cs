using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Bridge;

class Program
{
    const string SwitchConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=sqlswitch;Integrated Security=True;Max Pool Size=100";

    static async Task Main()
    {
        Console.Title = "Switch";

        var redSubscriptionStorage = new SqlSubscriptionStorage(() => new SqlConnection(SwitchConnectionString), "Red", new SqlDialect.MsSqlServer(), null);
        await redSubscriptionStorage.Install().ConfigureAwait(false);

        var greenSubscriptionStorage = new SqlSubscriptionStorage(() => new SqlConnection(SwitchConnectionString), "Green", new SqlDialect.MsSqlServer(), null);
        await greenSubscriptionStorage.Install().ConfigureAwait(false);

        #region SwitchConfig

        var switchConfig = new SwitchConfiguration();

        var blueSubscriptionStorage = new SqlSubscriptionStorage(
            () => new SqlConnection(SwitchConnectionString), 
            "Blue", 
            new SqlDialect.MsSqlServer(), 
            null);
        await blueSubscriptionStorage.Install().ConfigureAwait(false);

        switchConfig.AddPort<SqlServerTransport>("Blue", t =>
        {
            t.ConnectionString(ConnectionStrings.Blue);
        }).UseSubscriptionPersistence(blueSubscriptionStorage);

        
        switchConfig.AddPort<SqlServerTransport>("Red", t =>
        {
            t.ConnectionString(ConnectionStrings.Red);
        }).UseSubscriptionPersistence(redSubscriptionStorage);

        switchConfig.AddPort<SqlServerTransport>("Green", t =>
        {
            t.ConnectionString(ConnectionStrings.Green);
        }).UseSubscriptionPersistence(greenSubscriptionStorage);

        switchConfig.AutoCreateQueues();

        #endregion

        #region SwitchForwarding

        switchConfig.PortTable["Client"] = "Blue";
        switchConfig.PortTable["Sales"] = "Red";
        switchConfig.PortTable["Shipping"] = "Red";
        switchConfig.PortTable["Billing"] = "Green";

        #endregion

        SqlHelper.EnsureDatabaseExists(SwitchConnectionString);
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