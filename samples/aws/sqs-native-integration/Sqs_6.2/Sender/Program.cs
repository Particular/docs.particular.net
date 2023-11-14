using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

class Program
{
    #region NativeMessage
    private static readonly string MessageToSend = @"{""$type"" : ""NativeIntegration.Receiver.SomeNativeMessage, Receiver"", ""ThisIsTheMessage"": ""Hello world!""}";
    #endregion

    static async Task Main()
    {
        Console.Title = "Samples.Sqs.NativeIntegration";

        while (true) {
            Console.WriteLine("Enter 's' to send a message, enter 'exit' to stop");
            var line = Console.ReadLine();
            switch (line?.ToLowerInvariant())
            {
                case "exit":
                    return;
                case "s":

                    #region SendingANativeMessage                   
                    await SendTo(new Dictionary<string, MessageAttributeValue>
                    {                        
                        {"SomeRandomKey", new MessageAttributeValue {DataType = "String", StringValue = "something-random"}}, //optional attributes that the receiver might need
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
}