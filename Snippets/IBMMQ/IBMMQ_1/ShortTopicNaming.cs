using System;
using NServiceBus.Transport.IBMMQ;

#region ibmmq-custom-topic-naming-class

public sealed class ShortTopicNaming() : TopicNaming("APP")
{
    public override string GenerateTopicName(Type eventType)
    {
        return $"APP.{eventType.Name}".ToUpperInvariant();
    }
}

#endregion
