using System;
using System.Diagnostics;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using NUnit.Framework;

[TestFixture]
public class AzureHelper
{
    [Test]
    [Explicit]
    public void WriteOutData()
    {
        #region UsingHelpers

        WriteOutQueue("samples-azure-storagequeues-endpoint2");

        #endregion
    }

    #region WriteOutQueue

    static void WriteOutQueue(string queueName)
    {
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var queueClient = storageAccount.CreateCloudQueueClient();
        var queue = queueClient.GetQueueReference(queueName);
        var message = queue.PeekMessage();
        if (message != null)
        {
            Debug.WriteLine("Message contents");
            WriteOutMessage(message);
            return;
        }

        Console.WriteLine("No messages found in the 'samples-azure-storagequeues-endpoint2' queue. Execute 'Endpoint1' without running 'Endpoint2' and then try again.");
    }

    static void WriteOutMessage(CloudQueueMessage message)
    {
        var json = message.AsString;
        var byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (json.StartsWith(json))
        {
            json = json.Remove(0, byteOrderMarkUtf8.Length);
        }
        dynamic parsedJson = JsonConvert.DeserializeObject(json);
        Debug.WriteLine("CloudQueueMessage contents:");
        Debug.WriteLine(JsonConvert.SerializeObject((object) parsedJson, Formatting.Indented));
        var body = (string)parsedJson.Body;
        Debug.WriteLine("Serialized message body:");
        Debug.WriteLine(body.Base64Decode());
    }

    #endregion
}