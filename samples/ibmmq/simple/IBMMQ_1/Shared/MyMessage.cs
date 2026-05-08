using NServiceBus;

#region Message
public record MyMessage(string Data) : IMessage;
#endregion