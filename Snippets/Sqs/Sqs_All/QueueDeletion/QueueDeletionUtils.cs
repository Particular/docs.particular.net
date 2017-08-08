namespace SqsAll.QueueDeletion
{
    using System;
    using System.Threading.Tasks;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using SqsAll.QueueCreation;
    using Sqs_All;

    #region sqs-delete-queues

    public static class QueueDeletionUtils
    {
        public static async Task DeleteAllQueues()
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                await DeleteQueues(client)
                    .ConfigureAwait(false);
            }
        }

        static async Task DeleteQueues(IAmazonSQS client)
        {
            var queuesToDelete = await client.ListQueuesAsync("")
                .ConfigureAwait(false);
            var numberOfQueuesFound = queuesToDelete.QueueUrls.Count;
            var deletionTasks = new Task[numberOfQueuesFound + 1];

            for (var i = 0; i < numberOfQueuesFound; i++)
            {
                if (i % 200 == 0) // make sure we don't get throttled
                {
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
                deletionTasks[i] = client.DeleteQueueAsync(queuesToDelete.QueueUrls[i]);
            }

            // queue deletion can take up to 60 seconds
            deletionTasks[numberOfQueuesFound] = Task.Delay(TimeSpan.FromSeconds(60));
            await Task.WhenAll(deletionTasks)
                .ConfigureAwait(false);

            if (numberOfQueuesFound == 1000)
            {
                await DeleteQueues(client)
                    .ConfigureAwait(false);
            }
        }

        public static async Task DeleteQueue(string queueName)
        {
            try
            {
                using (var client = ClientFactory.CreateSqsClient())
                {
                    var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName);
                    var queueUrlResponse = await client.GetQueueUrlAsync(sqsQueueName)
                        .ConfigureAwait(false);
                    await client.DeleteQueueAsync(queueUrlResponse.QueueUrl)
                        .ConfigureAwait(false);
                }
            }
            catch (QueueDoesNotExistException)
            {
            }
        }
    }

    #endregion
}
