using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

class Program
{
    static readonly string MessageToSend = new XDocument(new XElement("NativeIntegration.Receiver.SomeNativeMessage", new XElement("ThisIsTheMessage", "Hello!"))).ToString();

    static async Task Main()
    {
        Console.Title = "Samples.Sqs.NativeIntegration";

        while (true) {
            Console.WriteLine("Enter 'S' to send a message, enter 'exit' to stop");
            var line = Console.ReadLine();
            switch (line?.ToLowerInvariant())
            {
                case "exit":
                    return;
                case "s":

                    #region SendingANativeMessage                   
                    await SendTo(new Dictionary<string, MessageAttributeValue>
                    {                        
                        {"NServiceBus.AmazonSQS.Headers", new MessageAttributeValue {DataType = "String", StringValue = "{}"}}, //required for native integration
                        {"SomeRandomKey", new MessageAttributeValue {DataType = "String", StringValue = "something-random"}}, //other optional attributes that the receiver might need
                    }, MessageToSend);
                    #endregion
                    Console.WriteLine("Message was sent.");
                    break;
            }
        }
    }

    static async Task SendTo(Dictionary<string, MessageAttributeValue> messageAttributeValues, string message)
    {
        using (var sqsClient = new AmazonSQSClient())
        {
            var getQueueUrlResponse = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest
            {
                QueueName = "Samples-Sqs-SimpleReceiver" // sanitized queue name
            }).ConfigureAwait(false);            

            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = getQueueUrlResponse.QueueUrl,
                MessageAttributes = messageAttributeValues,
                MessageBody = message
            };

            await sqsClient.SendMessageAsync(sendMessageRequest).ConfigureAwait(false);
        }
    }

    #region PopulatingNativeReplyToAddress
    static string CreateHeadersWithReply()
    {
        var nsbHeaders = new Dictionary<string, string>
        {
            { "NServiceBus.ReplyToAddress", "my-native-endpoint" }, //optional - used to demo replying back to the native endpoint         
        };

        return JsonConvert.SerializeObject(nsbHeaders, Formatting.None);
    }
    #endregion
}