using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static class VerificationLogger
{
    static CloudTable table;
    static object locker = new object();

    static VerificationLogger()
    {
        var cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
        table = cloudTableClient.GetTableReference("MultiHostedEndpointsOutput");
        table.CreateIfNotExists();
    }

    public static void Write(string endpoint, string message)
    {
        lock (locker)
        {
            var tableOperation = TableOperation.Insert(new LogEntry(endpoint, message));
            table.Execute(tableOperation);
        }
    }

}