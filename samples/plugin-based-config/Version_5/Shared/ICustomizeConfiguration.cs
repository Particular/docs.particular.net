using NServiceBus;

public interface ICustomizeConfiguration
{
    void Run(BusConfiguration busConfiguration);
}