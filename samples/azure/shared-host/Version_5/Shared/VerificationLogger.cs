using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static class VerificationLogger
{
    static CloudTable table;
    static object locker = new object();

    static VerificationLogger()
    {
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var tableClient = storageAccount.CreateCloudTableClient();
        table = tableClient.GetTableReference("MultiHostedEndpointsOutput");
        table.CreateIfNotExists();
    }

    public static void Write(string endpoint, string message)
    {
        lock (locker)
        {
            var operation = TableOperation.Insert(new LogEntry(endpoint, message));
            table.Execute(operation);
        }
    }

}