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

        await WriteOutQueue("samples-azure-storagequeues-endpoint2");

        #endregion
    }

    #region WriteOutQueue

    static async Task WriteOutQueue(string queueName)
    {
        var queueClient = new QueueClient("UseDevelopmentStorage=true", queueName);
        PeekedMessage[] message = await queueClient.PeekMessagesAsync(1);
        if (message?.Length >= 1)
        {
            Console.WriteLine("Message contents");
            WriteOutMessage(message[0]);
            return;
        }

        Console.WriteLine($"No messages found in the '{queueName}' queue. Execute 'Endpoint1' without running 'Endpoint2' and then try again");
    }

    static void WriteOutMessage(PeekedMessage message)
    {
        var bytes = Convert.FromBase64String(message.MessageText);
        var json = Encoding.UTF8.GetString(bytes);

        dynamic parsedJson = JsonConvert.DeserializeObject(json);
        Console.WriteLine("Message contents:");
        Console.WriteLine(JsonConvert.SerializeObject((object) parsedJson, Formatting.Indented));

        var body = (string)parsedJson.Body;
        Console.WriteLine("Deserialized message body:");
        Console.WriteLine(body.Base64Decode());
    }

    #endregion
}