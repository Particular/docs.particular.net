using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
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
        var queueClient = new QueueClient("UseDevelopmentStorage=true", queueName);
        PeekedMessage[] message = await queueClient.PeekMessagesAsync(1)
            .ConfigureAwait(false);
        if (message != null)
        {
            Debug.WriteLine("Message contents");
            WriteOutMessage(message[0]);
            return;
        }

        Console.WriteLine("No messages found in the 'samples-azure-storagequeues-endpoint2' queue. Execute 'Endpoint1' without running 'Endpoint2' and then try again.");
    }

    static void WriteOutMessage(PeekedMessage message)
    {
        var bytes = Convert.FromBase64String(message.MessageText);
        var json = Encoding.UTF8.GetString(bytes);
        var byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (json.StartsWith(json))
        {
            json = json.Remove(0, byteOrderMarkUtf8.Length);
        }
        dynamic parsedJson = JsonConvert.DeserializeObject(json);
        Debug.WriteLine("Message contents:");
        Debug.WriteLine(JsonConvert.SerializeObject((object) parsedJson, Formatting.Indented));
        var body = (string)parsedJson.Body;
        Debug.WriteLine("Deserialized message body:");
        Debug.WriteLine(body.Base64Decode());
    }

    #endregion
}