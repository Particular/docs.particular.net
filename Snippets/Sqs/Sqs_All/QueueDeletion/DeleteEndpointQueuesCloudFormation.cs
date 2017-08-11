namespace SqsAll.QueueDeletion
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.CloudFormation;
    using Amazon.CloudFormation.Model;

    public class DeleteEndpointQueuesCloudFormation
    {
        #region sqs-delete-queues-for-endpoint-cloudformation

        public static async Task DeleteQueuesForEndpoint(string endpointName, string queueNamePrefix = null)
        {
            using (var client = ClientFactory.CreateCloudFormationClient())
            {
                var endpointNameWithPrefix = QueueNameHelper.GetSqsQueueName(endpointName, queueNamePrefix);

                var request = new DeleteStackRequest
                {
                    StackName = endpointNameWithPrefix,
                };


                await client.DeleteStackAsync(request)
                    .ConfigureAwait(false);


                var describeRequest = new DescribeStacksRequest
                {
                    StackName = endpointNameWithPrefix
                };
                StackStatus currentStatus = string.Empty;
                while (currentStatus != StackStatus.DELETE_COMPLETE)
                {
                    try
                    {
                        var response = await client.DescribeStacksAsync(describeRequest)
                            .ConfigureAwait(false);
                        var stack = response.Stacks.SingleOrDefault();
                        currentStatus = stack?.StackStatus;
                    }
                    catch (AmazonCloudFormationException)
                    {
                        Console.WriteLine("Stack does not exist");
                        return;
                    }
                }
            }
        }

        #endregion
    }
}