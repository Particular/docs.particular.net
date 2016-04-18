using NServiceBus;
#region IRunAfterEndpointStart
public interface IRunAfterEndpointStart
{
    void Run(IBus bus);
}
#endregion