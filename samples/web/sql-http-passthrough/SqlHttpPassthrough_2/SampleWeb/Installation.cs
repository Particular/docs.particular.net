using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public static class Installation
{
    public static async Task Run()
    {
        using (var connection = await ConnectionHelpers.OpenConnection(SqlHelper.ConnectionString)
            .ConfigureAwait(false))
        {
            var deduplicationManager = new DeduplicationManager(connection, "Deduplication");
            await deduplicationManager.Create().ConfigureAwait(false);
        }
    }
}