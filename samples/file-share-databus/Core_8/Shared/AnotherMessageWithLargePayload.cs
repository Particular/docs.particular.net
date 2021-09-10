using NServiceBus;

#region AnotherMessageWithLargePayload

public class AnotherMessageWithLargePayload :
    ICommand
{
    public byte[] LargeBlob { get; set; }
}

#endregion