using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

#region NativeMessage
var MessageToSend = @"{""$type"" : ""NativeIntegration.Receiver.SomeNativeMessage, Receiver"", ""ThisIsTheMessage"": ""Hello world!""}";
#endregion

Console.Title = "NativeIntegration";

while (true)
{
    Console.WriteLine("Press [s] to send a message or [ESC] to exit.");

    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.S)
    {
        #region SendingANativeMessage
        await SendTo(new Dictionary<string, MessageAttributeValue>
                    {
                        {"SomeKey", new MessageAttributeValue {DataType = "String", StringValue = "something"}}, //optional attributes that the receiver might need
                    }, MessageToSend);
        #endregion
        Console.WriteLine("Message was sent.");
    }
    else if (key.Key == ConsoleKey.Escape)
    {
        return;
    }
}

static async Task SendTo(Dictionary<string, MessageAttributeValue> messageAttributeValues, string message)
{
    using var sqsClient = new AmazonSQSClient();
    var getQueueUrlResponse = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest
    {
        QueueName = "Samples-Sqs-SimpleReceiver" // sanitized queue name
    });

    var sendMessageRequest = new SendMessageRequest
    {
        QueueUrl = getQueueUrlResponse.QueueUrl,
        MessageAttributes = messageAttributeValues,
        MessageBody = message
    };

    await sqsClient.SendMessageAsync(sendMessageRequest);
}