namespace SqsAll.QueueCreation
{
    using System;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Sqs_All;

    #region sqs-create-queues

    public static class QueueCreationUtils
    {
        static TimeSpan DefaultTimeToLive = TimeSpan.FromDays(4);

        public static async Task CreateQueue(string queueName, TimeSpan? maxTimeToLive = null)
        {
            try
            {
                using (var client = ClientFactory.CreateSqsClient())
                {
                    var sqsRequest = new CreateQueueRequest
                    {
                        QueueName = QueueNameHelper.GetSqsQueueName(queueName)
                    };
                    var value = Convert.ToInt32((maxTimeToLive ?? DefaultTimeToLive).TotalSeconds).ToString();
                    sqsRequest.Attributes.Add(QueueAttributeName.MessageRetentionPeriod, value);
                    await client.CreateQueueAsync(sqsRequest)
                        .ConfigureAwait(false);
                }
            }
            catch (QueueNameExistsException)
            {
            }
        }
    }

    #endregion
}