using NServiceBus;

public interface IRunBeforeEndpointStop
{
    void Run(IBus bus);
}