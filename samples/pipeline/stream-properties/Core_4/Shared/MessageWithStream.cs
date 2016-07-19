using System.IO;
using NServiceBus;

#region message-with-stream

[TimeToBeReceived("00:01:00")]
public class MessageWithStream :
    ICommand
{
    public string SomeProperty { get; set; }
    public Stream StreamProperty { get; set; }
}

#endregion
