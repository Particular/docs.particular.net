using System;
using NServiceBus;

#region message-definitions

[Serializable]
[SerializeWithBinary]
public class MessageWithBinary : IMessage
{
    public string SomeProperty { get; set; }
}
[SerializeWithJson]
public class MessageWithJson : IMessage
{
    public string SomeProperty { get; set; }
}

#endregion
