namespace SqsAll.QueueDeletion
{
    using System.Threading.Tasks;
    using Amazon.SQS.Model;
    using SqsAll.QueueCreation;
    using Sqs_All;

    public static class QueueExistenceUtils
    {
        public static async Task<bool> Exists(string queueName)
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                try
                {
                    var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName);
                    await client.GetQueueUrlAsync(sqsQueueName)
                        .ConfigureAwait(false);
                    return true;
                }
                catch (QueueDoesNotExistException)
                {
                    return false;
                }
            }
        }
    }
}