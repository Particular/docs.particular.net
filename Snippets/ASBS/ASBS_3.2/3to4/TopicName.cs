using NServiceBus;

class TopicName
{
    void Old()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region 3to4-asbs-topic-name-old
        var transport = new AzureServiceBusTransport("my connection string")
        {
            TopicName = "CustomTopicName"
        };
        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void New()
    {
        #region 3to4-asbs-topic-name-new
        var transport = new AzureServiceBusTransport("my connection string")
        {
            Topology = TopicTopology.Single("CustomTopicName")
        };
        #endregion
    }
}