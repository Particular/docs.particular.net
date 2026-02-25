using System;
using NServiceBus.Transport.IbmMq;

#region ibmmq-custom-topic-naming-class

public class ShortTopicNaming() : TopicNaming("APP")
{
    public override string GenerateTopicName(Type eventType)
    {
        return $"APP.{eventType.Name}".ToUpperInvariant();
    }
}

#endregion
