using NServiceBus;

namespace Endpoint2;

public record MyRequest(string Property) : IMessage;
