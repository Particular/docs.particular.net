using System;
using NServiceBus;

#region RequestMessage
public record RequestDataMessage(Guid DataId, string String) : IMessage;
#endregion