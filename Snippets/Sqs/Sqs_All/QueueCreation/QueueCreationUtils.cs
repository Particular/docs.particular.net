namespace SqsAll.QueueCreation
{
    using System;
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
                sqsRequest.Attributes.Add(QueueAttributeName.MessageRetentionPeriod, Convert.ToInt32((maxTimeToLive ?? DefaultTimeToLive).TotalSeconds).ToString());

                if (queueName.EndsWith(".fifo"))
                {
                    sqsRequest.Attributes.Add(QueueAttributeName.FifoQueue, "true");
                    sqsRequest.Attributes.Add(QueueAttributeName.DelaySeconds, "900");
                }

                try
                {
                    var createQueueResponse = await client.CreateQueueAsync(sqsRequest)
                        .ConfigureAwait(false);
                }
                catch (QueueNameExistsException)
                {
                }
            }
        }
    }

    #endregion
}