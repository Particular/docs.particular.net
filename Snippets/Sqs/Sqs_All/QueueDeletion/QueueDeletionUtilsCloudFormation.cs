﻿namespace SqsAll.QueueDeletion
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.CloudFormation;
    using Amazon.CloudFormation.Model;

    #region sqs-delete-queues-cloudformation

    public static class QueueDeletionUtilsCloudFormation
    {
        public static async Task DeleteQueue(string queueName, string queueNamePrefix = null)
        {
            using (var client = ClientFactory.CreateCloudFormationClient())
            {
                var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix);

                var request = new DeleteStackRequest
                {
                    StackName = sqsQueueName,
                };


                await client.DeleteStackAsync(request);

                var describeRequest = new DescribeStacksRequest
                {
                    StackName = sqsQueueName
                };
                StackStatus currentStatus = string.Empty;
                while (currentStatus != StackStatus.DELETE_IN_PROGRESS) // in progress is enough, no need to wait for completion
                {
                    try
                    {
                        var response = await client.DescribeStacksAsync(describeRequest);
                        var stack = response.Stacks.SingleOrDefault();
                        currentStatus = stack?.StackStatus;
                        await Task.Delay(1000);
                    }
                    catch (AmazonCloudFormationException)
                    {
                        Console.WriteLine("Stack does not exist");
                        return;
                    }
                }
            }
        }
    }

    #endregion
}