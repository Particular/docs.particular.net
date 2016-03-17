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
        CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
        CloudQueue queue = queueClient.GetQueueReference(queueName);
        CloudQueueMessage message = queue.PeekMessage();
        Debug.WriteLine("Message contents");
        WriteOutMessage(message);
    }

    static void WriteOutMessage(CloudQueueMessage message)
    {
        string json = message.AsString;
        string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (json.StartsWith(json))
        {
            json = json.Remove(0, byteOrderMarkUtf8.Length);
        }
        dynamic parsedJson = JsonConvert.DeserializeObject(json);
        Debug.WriteLine("CloudQueueMessage contents:");
        Debug.WriteLine(JsonConvert.SerializeObject((object) parsedJson, Formatting.Indented));
        string body = (string)parsedJson.Body;
        Debug.WriteLine("Serialized message body:");
        Debug.WriteLine(body.Base64Decode());
    }

    #endregion
}

public static class Extensions
{
    public static string Base64Decode(this string encodedBody)
    {
        byte[] bytes = Convert.FromBase64String(encodedBody);
        return Encoding.UTF8.GetString(bytes);
    }
    
}