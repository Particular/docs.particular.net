using System;
using Microsoft.WindowsAzure.Storage.Table;

class LogEntry :
    TableEntity
{
    public LogEntry(string endpoint, string message)
    {
        PartitionKey = endpoint;
        RowKey = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Message = message;
    }

    public string Message { get; set; }
}