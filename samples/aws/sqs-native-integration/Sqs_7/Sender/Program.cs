using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

#region NativeMessage
var MessageToSend = @"{""$type"" : ""NativeIntegration.Receiver.SomeNativeMessage, Receiver"", ""ThisIsTheMessage"": ""Hello world!""}";
#endregion

Console.Title = "Samples.Sqs.NativeIntegration";

while (true)
{
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
                        {"SomeKey", new MessageAttributeValue {DataType = "String", StringValue = "something"}}, //optional attributes that the receiver might need
                    }, MessageToSend);
            #endregion
            Console.WriteLine("Message was sent.");
            break;
    }
}

static async Task SendTo(Dictionary<string, MessageAttributeValue> messageAttributeValues, string message)
{
    using (var sqsClient = new AmazonSQSClient())
    {
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
}