using NServiceBus;

namespace Shared;

public record MyResponse(string Property) : IMessage;