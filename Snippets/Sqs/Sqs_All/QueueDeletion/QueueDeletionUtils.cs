namespace SqsAll.QueueDeletion
{
    using System;
    using System.Threading.Tasks;
    using Amazon.Runtime;
    using Amazon.SQS;
    using Amazon.SQS.Model;

    public static class QueueDeletionUtils
    {
        #region sqs-delete-queues

        public static async Task DeleteQueue(string queueName, string queueNamePrefix = null)
        {
            try
            {
                using (var client = ClientFactory.CreateSqsClient())
                {
                    var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix);
                    var queueUrlResponse = await client.GetQueueUrlAsync(sqsQueueName);
                    await client.DeleteQueueAsync(queueUrlResponse.QueueUrl);
                }
            }
            catch (QueueDoesNotExistException)
            {
            }
        }

        #endregion

        #region sqs-delete-all-queues

        public static async Task DeleteAllQueues(string queueNamePrefix = null)
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                await DeleteQueues(client, queueNamePrefix);
            }
        }

        static async Task DeleteQueues(IAmazonSQS client, string queueNamePrefix = null)
        {
            var queuesToDelete = await client.ListQueuesAsync(queueNamePrefix);
            var numberOfQueuesFound = queuesToDelete.QueueUrls.Count;
            var deletionTasks = new Task[numberOfQueuesFound + 1];

            for (var i = 0; i < numberOfQueuesFound; i++)
            {
                deletionTasks[i] = RetryDeleteOnThrottle(client, queuesToDelete.QueueUrls[i], TimeSpan.FromSeconds(10), 6);
            }

            // queue deletion can take up to 60 seconds
            deletionTasks[numberOfQueuesFound] = numberOfQueuesFound > 0 ? Task.Delay(TimeSpan.FromSeconds(60)) : Task.CompletedTask;

            await Task.WhenAll(deletionTasks);

            if (numberOfQueuesFound == 1000)
            {
                await DeleteQueues(client);
            }
        }

        static async Task RetryDeleteOnThrottle(IAmazonSQS client, string queueUrl, TimeSpan delay, int maxRetryAttempts, int retryAttempts = 0)
        {
            try
            {
                await client.DeleteQueueAsync(queueUrl);
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
                    await Task.Delay(attempts);
                    await RetryDeleteOnThrottle(client, queueUrl, delay, maxRetryAttempts, ++retryAttempts);
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

        #endregion
    }
}