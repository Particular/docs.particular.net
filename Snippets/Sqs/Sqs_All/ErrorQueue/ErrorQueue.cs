namespace SqsAll.ErrorQueue
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Newtonsoft.Json;
    using SqsAll.QueueCreation;
    using Sqs_All;

    public static class ErrorQueue
    {

        static async Task Usage()
        {
            #region sqs-return-to-source-queue-usage

            await ReturnMessageToSourceQueue(
                    errorQueueName: "error",
                    messageId: "c390a6fb-4fb5-46da-927d-a156f75739eb")
                .ConfigureAwait(false);

            #endregion
        }

        #region sqs-return-to-source-queue

        public static async Task ReturnMessageToSourceQueue(string errorQueueName, string messageId)
        {
            var path = QueueNameHelper.GetSqsQueueName($"{errorQueueName}");
            using (var client = ClientFactory.CreateSqsClient())
            {
                var queueUrlResponse = await client.GetQueueUrlAsync(path)
                    .ConfigureAwait(false);
                var queueUrl = queueUrlResponse.QueueUrl;

                await InspectMessagesUntilFound(client, messageId, queueUrl)
                    .ConfigureAwait(false);
            }
        }

        static async Task InspectMessagesUntilFound(IAmazonSQS client, string messageId, string queueUrl)
        {
            var receivedMessages = await client.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,
                    WaitTimeSeconds = 1,
                    MaxNumberOfMessages = 10,
                })
                .ConfigureAwait(false);

            if (receivedMessages.Messages.Count == 0)
            {
                return;
            }

            foreach (var message in receivedMessages.Messages)
            {
                if (message.MessageId != messageId)
                {
                    continue;
                }

                var failedQueueName = ReadFailedQueueHeader(message);
                var failedQueueUrlResponse = await client.GetQueueUrlAsync(failedQueueName)
                    .ConfigureAwait(false);
                var failedQueueUrl = failedQueueUrlResponse.QueueUrl;
                // Large message don't need to be handled separately since the S3BodyKey is preserved
                await client.SendMessageAsync(new SendMessageRequest(failedQueueUrl, message.Body)) // what to do with the attributes?
                    .ConfigureAwait(false);
                await client.DeleteMessageAsync(queueUrl, message.ReceiptHandle)
                    .ConfigureAwait(false);
                return;
            }

            await InspectMessagesUntilFound(client, messageId, queueUrl)
                .ConfigureAwait(false);
        }

        static string ReadFailedQueueHeader(Message message)
        {
            var headers = ExtractHeaders(message);
            var queueName = headers.Single(x => x.Key == "NServiceBus.FailedQ").Value;
            return QueueNameHelper.GetSqsQueueName($"{queueName}");
        }

        static Dictionary<string, string> ExtractHeaders(Message message)
        {
            var transportMessage = JsonConvert.DeserializeObject<TransportMessage>(message.Body);
            return transportMessage.Headers;
        }

        public class HeaderInfo
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        #endregion
    }
}