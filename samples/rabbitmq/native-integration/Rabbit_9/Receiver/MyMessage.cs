using NServiceBus;

#region DefineNSBMessage

public class MyMessage :
    IMessage
{
    public string SomeProperty { get; set; }
}

#endregion