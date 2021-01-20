using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;

class Program
{
    static readonly string MessageToSend = new XDocument(new XElement("SomeNativeMessage", new XElement("ThisIsTheMessage", "Hello!"))).ToString();
    static async Task Main()
    {
        Console.Title = "Samples.Sqs.NativeIntegration";

        while (true) {
            Console.WriteLine("Enter 'S' to send a message, enter 'exit' to stop");
            var line = Console.ReadLine(); // Get string from user
            if (line == "exit") // Check string
            {
                break;
            }

            if (line == "S")
            {
                #region SendingANativeMessage
                await SendTo(new Dictionary<string, MessageAttributeValue>
                {
                    {"MessageTypeFullName", new MessageAttributeValue {DataType = "String", StringValue = "SomeNativeMessage"}}, // required for native integration
                    //{"S3BodyKey", new MessageAttributeValue {DataType = "String", StringValue = "s3bodykey"}}, // optional for native integration
                    {"SomeRandomKey", new MessageAttributeValue {DataType = "String", StringValue = "something-random"}},
                    {"AnotherRandomKey", new MessageAttributeValue {DataType = "String", StringValue = "something-else-thats-random"}},
                }, MessageToSend);
                #endregion
                Console.WriteLine("Message was sent.");
            }
        }
    }

    public static async Task SendTo(Dictionary<string, MessageAttributeValue> messageAttributeValues, string message)
    {
        using (var sqsClient = new AmazonSQSClient())
        {
            var getQueueUrlResponse = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest
            {
                QueueName = "Samples-Sqs-SimpleReceiver" // sanitized queue name
            }).ConfigureAwait(false);

            var body = Convert.ToBase64String(Encoding.Unicode.GetBytes(message));

            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = getQueueUrlResponse.QueueUrl,
                MessageAttributes = messageAttributeValues,
                MessageBody = body
            };

            await sqsClient.SendMessageAsync(sendMessageRequest).ConfigureAwait(false);
        }
    }
}