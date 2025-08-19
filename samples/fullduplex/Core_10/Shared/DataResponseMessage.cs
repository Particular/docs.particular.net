using NServiceBus;
using System;

#region ResponseMessage
public record DataResponseMessage(Guid DataId, string String) : IMessage;
#endregion
