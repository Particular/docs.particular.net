using NServiceBus;
using System;

#region ResponseMessage
// C# 12+ Primary constructor with record for immutable message
public record DataResponseMessage(Guid DataId, string String) : IMessage;
#endregion
