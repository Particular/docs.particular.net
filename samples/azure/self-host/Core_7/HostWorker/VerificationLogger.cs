using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static class VerificationLogger
{
    static AsyncLazy<CloudTable> lazyTable;
    static SemaphoreSlim semaphore = new SemaphoreSlim(1);

    static VerificationLogger()
    {
        lazyTable = new AsyncLazy<CloudTable>(async () =>
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("HostWorker.ConnectionString"));
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var t = cloudTableClient.GetTableReference("SelfHostedEndpointsOutput");
            await t.CreateIfNotExistsAsync().ConfigureAwait(false);
            return t;
        });
    }

    public static async Task Write(string endpoint, string message)
    {
        try
        {
            await semaphore.WaitAsync()
                .ConfigureAwait(false);
            var tableOperation = TableOperation.Insert(new LogEntry(endpoint, message));
            var table = await lazyTable;
            await table.ExecuteAsync(tableOperation)
                .ConfigureAwait(false);
        }
        finally
        {
            semaphore.Release();
        }
    }
}