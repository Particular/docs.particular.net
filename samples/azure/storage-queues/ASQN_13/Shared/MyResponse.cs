using NServiceBus;

namespace Endpoint2;

public record MyResponse(string Property) : IMessage;