namespace SqsAll.QueueDeletion
{
    using System;
    using System.Threading.Tasks;
    using Amazon.Runtime;
    using Amazon.SQS;
    using Amazon.SQS.Model;

    #region sqs-delete-queues

    public static class QueueDeletionUtils
    {
        public static async Task DeleteAllQueues(string queueNamePrefix = null)
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                await DeleteQueues(client, queueNamePrefix)
                    .ConfigureAwait(false);
            }
        }

        static async Task DeleteQueues(IAmazonSQS client, string queueNamePrefix = null)
        {
            var queuesToDelete = await client.ListQueuesAsync(queueNamePrefix)
                .ConfigureAwait(false);
            var numberOfQueuesFound = queuesToDelete.QueueUrls.Count;
            var deletionTasks = new Task[numberOfQueuesFound + 1];

            for (var i = 0; i < numberOfQueuesFound; i++)
            {
                deletionTasks[i] = RetryDeleteOnThrottle(client, queuesToDelete.QueueUrls[i], TimeSpan.FromSeconds(10), 6);
            }

            // queue deletion can take up to 60 seconds
            deletionTasks[numberOfQueuesFound] = numberOfQueuesFound > 0 ? Task.Delay(TimeSpan.FromSeconds(60)) : Task.FromResult(0);

            await Task.WhenAll(deletionTasks)
                .ConfigureAwait(false);

            if (numberOfQueuesFound == 1000)
            {
                await DeleteQueues(client)
                    .ConfigureAwait(false);
            }
        }

        public static async Task DeleteQueue(string queueName, string queueNamePrefix = null, bool preTruncateQueueNames = false)
        {
            try
            {
                using (var client = ClientFactory.CreateSqsClient())
                {
                    var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix, preTruncateQueueNames);
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

        static async Task RetryDeleteOnThrottle(IAmazonSQS client, string queueUrl, TimeSpan delay, int maxRetryAttempts, int retryAttempts = 0)
        {
            try
            {
                await client.DeleteQueueAsync(queueUrl).ConfigureAwait(false);
            }
            catch (AmazonServiceException ex) when (ex.ErrorCode == "AWS.SimpleQueueService.NonExistentQueue")
            {
                // ignore
            }
            catch (AmazonServiceException ex) when (ex.ErrorCode == "RequestThrottled")
            {
                if (retryAttempts < maxRetryAttempts)
                {
                    var attempts = TimeSpan.FromMilliseconds(Convert.ToInt32(delay.TotalMilliseconds * (retryAttempts + 1)));
                    Console.WriteLine($"Retry {queueUrl} {retryAttempts}/{maxRetryAttempts} with delay {attempts}.");
                    await Task.Delay(attempts).ConfigureAwait(false);
                    await RetryDeleteOnThrottle(client, queueUrl, delay, maxRetryAttempts, ++retryAttempts).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine($"Unable to delete {queueUrl}. Max retry attempts reached.");
                }
            }
            catch (AmazonServiceException ex)
            {
                Console.WriteLine($"Unable to delete {queueUrl} on {retryAttempts}/{maxRetryAttempts}. Reason: {ex.Message}");
            }
        }
    }

    #endregion
}
