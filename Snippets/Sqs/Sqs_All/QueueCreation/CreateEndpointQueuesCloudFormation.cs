namespace SqsAll.QueueCreation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Amazon.CloudFormation;
    using Amazon.CloudFormation.Model;

    static class CreateEndpointQueuesCloudFormation
    {
        static async Task Usage()
        {
            #region sqs-create-queues-endpoint-usage-cloudformation

            await CreateQueuesForEndpoint(
                    endpointName: "myendpoint",
                    templatePath: @".\CreateQueuesEndpoint.json",
                    maxTimeToLive: TimeSpan.FromDays(2),
                    queueNamePrefix: "PROD",
                    includeRetries: true /* required for V5 and below */)
                .ConfigureAwait(false);

            #endregion
        }

        #region sqs-create-queues-for-endpoint-cloudformation

        public static async Task CreateQueuesForEndpoint(string endpointName, string templatePath, TimeSpan? maxTimeToLive = null, string queueNamePrefix = null, bool includeRetries = false, string delayedDeliveryMethod = "Native")
        {
            using (var client = ClientFactory.CreateCloudFormationClient())
            {
                var endpointNameWithPrefix = QueueNameHelper.GetSqsQueueName(endpointName, queueNamePrefix);
                var request = new CreateStackRequest
                {
                    StackName = endpointNameWithPrefix,
                    Parameters = new List<Parameter>
                    {
                        new Parameter
                        {
                            ParameterKey = "EndpointName",
                            ParameterValue = endpointNameWithPrefix
                        },
                        new Parameter
                        {
                            ParameterKey = "MaxTimeToLive",
                            ParameterValue = Convert.ToInt32((maxTimeToLive ?? QueueCreationUtils.DefaultTimeToLive).TotalSeconds).ToString()
                        },
                        new Parameter
                        {
                            ParameterKey = "IncludeRetries",
                            ParameterValue = includeRetries.ToString()
                        },
                        new Parameter
                        {
                            ParameterKey = "DelayedDeliveryMethod",
                            ParameterValue = delayedDeliveryMethod
                        },
                    },
                    TemplateBody = File.ReadAllText(templatePath)
                };

                await client.CreateStackAsync(request)
                    .ConfigureAwait(false);

                var describeRequest = new DescribeStacksRequest
                {
                    StackName = endpointNameWithPrefix
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

        #endregion
    }
}