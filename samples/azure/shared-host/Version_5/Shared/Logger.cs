namespace Shared
{
    using System;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public static class Logger
    {
        static CloudTable table;
        static object locker = new object();

        static Logger()
        {
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            var tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference("MultiHostedEndpointsOutput");
            table.CreateIfNotExists();
        }

        public static void WriteLine(string endpoint, string message)
        {
            lock (locker)
            {
                var operation = TableOperation.Insert(new LogEntry(endpoint, message));
                table.Execute(operation);
            }
        }

    }

    class LogEntry : TableEntity
    {
        public LogEntry(string endpoint, string message)
        {
            PartitionKey = endpoint;
            RowKey = DateTime.Now.ToString();
            Message = message;
        }

        public string Message { get; set; }
    }
}