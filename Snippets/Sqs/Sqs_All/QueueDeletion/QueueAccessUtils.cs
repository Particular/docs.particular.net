namespace SqsAll.QueueDeletion
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amazon.SQS.Model;

    public static class QueueAccessUtils
    {
        public static async Task<GetQueueAttributesResponse> Exists(string queueName, List<string> attributeNames = null)
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                try
                {
                    var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName);
                    var response = await client.GetQueueUrlAsync(queueName).ConfigureAwait(false);
                    var attributesResponse = await client.GetQueueAttributesAsync(response.QueueUrl, attributeNames ?? new List<string>())
                        .ConfigureAwait(false);
                    return attributesResponse;
                }
                catch (QueueDoesNotExistException)
                {
                    return null;
                }
            }
        }
    }
}