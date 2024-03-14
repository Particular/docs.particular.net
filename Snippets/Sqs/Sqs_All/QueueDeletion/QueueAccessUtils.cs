namespace SqsAll.QueueDeletion
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amazon.SQS.Model;

    public static class QueueAccessUtils
    {
        public static async Task<GetQueueAttributesResponse> Exists(string queueName, string queueNamePrefix = null, List<string> attributeNames = null)
        {
            using (var client = ClientFactory.CreateSqsClient())
            {
                try
                {
                    var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix);
                    var response = await client.GetQueueUrlAsync(sqsQueueName);
                    var attributesResponse = await client.GetQueueAttributesAsync(response.QueueUrl, attributeNames ?? new List<string>());
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