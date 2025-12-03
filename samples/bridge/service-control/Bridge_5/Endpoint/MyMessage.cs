using NServiceBus;

record MyMessage(string Id) : IMessage;