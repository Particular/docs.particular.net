using System;
using NServiceBus;

#region message-definitions

[Serializable]
[SerializeWithXml]
public class MessageWithXml :
    IMessage
{
    public string SomeProperty { get; set; }
}

[SerializeWithJson]
public class MessageWithJson :
    IMessage
{
    public string SomeProperty { get; set; }
}

#endregion
