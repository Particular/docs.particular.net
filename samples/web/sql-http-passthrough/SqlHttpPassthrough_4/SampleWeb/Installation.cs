using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public static class Installation
{
    public static async Task Run()
    {
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString)
            .ConfigureAwait(false))
        {
            var dedupeManager = new DedupeManager(connection, "Deduplication");
            await dedupeManager.Create().ConfigureAwait(false);
        }
    }
}