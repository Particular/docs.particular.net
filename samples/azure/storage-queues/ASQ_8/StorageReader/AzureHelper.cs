using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using NUnit.Framework;

[TestFixture]
public class AzureHelper
{
    [Test]
    [Explicit]
    public async Task WriteOutData()
    {
        #region UsingHelpers

        await WriteOutQueue("samples-azure-storagequeues-endpoint2")
            .ConfigureAwait(false);

        #endregion
    }

    #region WriteOutQueue

    static async Task WriteOutQueue(string queueName)
    {
        var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        var queueClient = storageAccount.CreateCloudQueueClient();
        var queue = queueClient.GetQueueReference(queueName);
        var message = await queue.PeekMessageAsync()
            .ConfigureAwait(false);
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