using System;
using NServiceBus;

#region RequestMessage
// C# 12+ Primary constructor with record for immutable message
public record RequestDataMessage(Guid DataId, string String) : IMessage;
#endregion