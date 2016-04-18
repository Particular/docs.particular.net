using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static class VerificationLogger
{
    static CloudTable table;
    static object locker = new object();

    static VerificationLogger()
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
        table = tableClient.GetTableReference("MultiHostedEndpointsOutput");
        table.CreateIfNotExists();
    }

    public static void Write(string endpoint, string message)
    {
        lock (locker)
        {
            TableOperation operation = TableOperation.Insert(new LogEntry(endpoint, message));
            table.Execute(operation);
        }
    }

}