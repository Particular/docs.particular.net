using NServiceBus;

namespace Shared;

public record MyRequest(string Property) : IMessage;
