using System;
using System.Collections.Generic;
using System.Text;

namespace SqsAll.NativeSend
{
    using System.Threading.Tasks;
    using Amazon.S3;
    using Amazon.SQS;
    using System.IO;
    using Amazon.S3.Model;

    public static class NativeSend
    {
        static async Task Usage(IAmazonSQS client, IAmazonS3 s3Client)
        {
            #region sqs-nativesend-usage

            await SendMessage(
                sqsClient: client,
                queue: "samples-sqs-nativeintegration",
                messageBody: "{Property:'PropertyValue'}",
                headers: new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", "MessageTypeToSend"},
                    {"NServiceBus.MessageId", "99C7320B-A645-4C74-95E8-857EAB98F4F9"}
                }
            )
            .ConfigureAwait(false);

            #endregion

            #region sqs-nativesend-large-usage

            await SendLargeMessage(
                    sqsClient: client,
                    s3Client: s3Client,
                    queue: "samples-sqs-nativeintegration-large",
                    s3Prefix: "s3prefix",
                    bucketName: "bucketname",
                    messageBody: "{Property:'PropertyValue'}",
                    headers: new Dictionary<string, string>
                    {
                        {"NServiceBus.EnclosedMessageTypes", "MessageTypeToSend"},
                        {"NServiceBus.MessageId", "99C7320B-A645-4C74-95E8-857EAB98F4F9"}
                    }
                )
                .ConfigureAwait(false);

            #endregion
        }

        #region sqs-nativesend

        public static async Task SendMessage(IAmazonSQS sqsClient, string queue, string messageBody, Dictionary<string, string> headers)
        {
            var bodyBytes = Encoding.UTF8.GetBytes(messageBody);
            var base64Body = Convert.ToBase64String(bodyBytes);
            var serializeMessage = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Headers = headers,
                Body = base64Body,
            });
            var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queue)
                .ConfigureAwait(false);
            await sqsClient.SendMessageAsync(queueUrlResponse.QueueUrl, serializeMessage)
                .ConfigureAwait(false);
        }

        #endregion

        #region sqs-nativesend-large

        public static async Task SendLargeMessage(IAmazonSQS sqsClient, IAmazonS3 s3Client, string queue, string s3Prefix, string bucketName, string messageBody, Dictionary<string, string> headers)
        {
            var bodyBytes = Encoding.UTF8.GetBytes(messageBody);
            var key = $"{s3Prefix}/{headers["NServiceBus.MessageId"]}";
            using (var bodyStream = new MemoryStream(bodyBytes))
            {
                await s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = bucketName,
                    InputStream = bodyStream,
                    Key = key
                }).ConfigureAwait(false);
            }
            var serializeMessage = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Headers = headers,
                Body = string.Empty,
                S3BodyKey = key
            });
            var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queue)
                .ConfigureAwait(false);
            await sqsClient.SendMessageAsync(queueUrlResponse.QueueUrl, serializeMessage)
                .ConfigureAwait(false);
        }

        #endregion
    }
}