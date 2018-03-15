namespace SqsAll.QueueCreation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;

    #region sqs-create-queues

    public static class QueueCreationUtils
    {
        public static TimeSpan DefaultTimeToLive = TimeSpan.FromDays(4);

        public static async Task CreateQueue(string queueName, TimeSpan? maxTimeToLive = null, string queueNamePrefix = null)
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                var sqsRequest = new CreateQueueRequest
                {
                    QueueName = QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix),
                };

                var isFifoQueue = queueName.EndsWith(".fifo");

                if (isFifoQueue)
                {
                    sqsRequest.Attributes = new Dictionary<string, string>
                    {
                        {QueueAttributeName.FifoQueue, "true"},
                    };
                }

                try
                {
                    var createQueueResponse = await client.CreateQueueAsync(sqsRequest)
                        .ConfigureAwait(false);

                    var sqsAttributesRequest = new SetQueueAttributesRequest
                    {
                        QueueUrl = createQueueResponse.QueueUrl
                    };

                    sqsAttributesRequest.Attributes.Add(QueueAttributeName.MessageRetentionPeriod, Convert.ToInt32((maxTimeToLive ?? DefaultTimeToLive).TotalSeconds).ToString());

                    if (isFifoQueue)
                    {
                        sqsAttributesRequest.Attributes.Add(QueueAttributeName.DelaySeconds, "900");
                    }

                    await client.SetQueueAttributesAsync(sqsAttributesRequest).ConfigureAwait(false);
                }
                catch (QueueNameExistsException)
                {
                }
            }
        }
    }

    #endregion
}