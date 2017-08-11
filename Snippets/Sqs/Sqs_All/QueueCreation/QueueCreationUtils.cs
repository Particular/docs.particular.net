namespace SqsAll.QueueCreation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.CloudFormation;
    using Amazon.CloudFormation.Model;
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
                    QueueName = QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix)
                };
                var value = Convert.ToInt32((maxTimeToLive ?? DefaultTimeToLive).TotalSeconds).ToString();
                sqsRequest.Attributes.Add(QueueAttributeName.MessageRetentionPeriod, value);
                try
                {
                    await client.CreateQueueAsync(sqsRequest)
                        .ConfigureAwait(false);
                }
                catch (QueueNameExistsException)
                {
                }
            }
        }
    }

    #endregion

    #region sqs-create-queues-cloudformation

    public static class QueueCreationUtilsCloudFormation
    {
        static TimeSpan DefaultTimeToLive = TimeSpan.FromDays(4);

        public static async Task CreateQueue(string queueName, string templatePath, TimeSpan? maxTimeToLive = null, string queueNamePrefix = null)
        {
            using (var client = ClientFactory.CreateCloudFormationClient())
            {
                var sqsQueueName = QueueNameHelper.GetSqsQueueName(queueName, queueNamePrefix);
                var request = new CreateStackRequest
                {
                    StackName = sqsQueueName,
                    Parameters = new List<Parameter>
                    {
                        new Parameter
                        {
                            ParameterKey = "QueueName",
                            ParameterValue = sqsQueueName
                        },
                        new Parameter
                        {
                            ParameterKey = "MaxTimeToLive",
                            ParameterValue = Convert.ToInt32((maxTimeToLive ?? DefaultTimeToLive).TotalSeconds).ToString()
                        }
                    },
                    TemplateBody = File.ReadAllText(templatePath)
                };

                await client.CreateStackAsync(request)
                    .ConfigureAwait(false);

                var describeRequest = new DescribeStacksRequest
                {
                    StackName = sqsQueueName
                };
                StackStatus currentStatus = string.Empty;
                while (currentStatus != StackStatus.CREATE_COMPLETE)
                {
                    var response = await client.DescribeStacksAsync(describeRequest)
                        .ConfigureAwait(false);
                    var stack = response.Stacks.SingleOrDefault();
                    currentStatus = stack?.StackStatus;
                }
            }
        }
    }

    #endregion
}