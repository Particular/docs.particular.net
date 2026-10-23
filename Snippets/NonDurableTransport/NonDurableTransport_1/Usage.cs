using NServiceBus;
using System;
using Microsoft.Extensions.Time.Testing;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableTransport

        endpointConfiguration.UseTransport(new NonDurableTransport());

        #endregion
    }

    void SharedBroker(EndpointConfiguration endpointConfigurationA, EndpointConfiguration endpointConfigurationB)
    {
        #region NonDurableTransport-SharedBroker

        var sharedBroker = new NonDurableBroker();

        endpointConfigurationA.UseTransport(new NonDurableTransport(new NonDurableTransportOptions(sharedBroker)));
        endpointConfigurationB.UseTransport(new NonDurableTransport(new NonDurableTransportOptions(sharedBroker)));

        #endregion
    }

    void InlineExecution(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableTransport-InlineExecution

        var transport = new NonDurableTransport(new NonDurableTransportOptions
        {
            InlineExecution = new InlineExecutionOptions
            {
                MoveToErrorQueueOnFailure = true
            }
        });

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void Simulation(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableTransport-Simulation

        var broker = new NonDurableBroker(new NonDurableBrokerOptions
        {
            Send =
            {
                Mode = NonDurableSimulationMode.Delay,
                RateLimit = new NonDurableRateLimitOptions
                {
                    PermitLimit = 1,
                    Window = TimeSpan.FromSeconds(5)
                }
            },
            Receive =
            {
                Mode = NonDurableSimulationMode.Reject,
                RateLimit = new NonDurableRateLimitOptions
                {
                    PermitLimit = 0,
                    Window = TimeSpan.FromSeconds(30)
                }
            }
        });

        endpointConfiguration.UseTransport(new NonDurableTransport(new NonDurableTransportOptions(broker)));

        #endregion
    }

    void SimulatedTime(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableTransport-SimulatedTime

        var simulatedTime = new FakeTimeProvider(DateTimeOffset.UtcNow);

        var broker = new NonDurableBroker(new NonDurableBrokerOptions
        {
            TimeProvider = simulatedTime,
            Send =
            {
                Mode = NonDurableSimulationMode.Delay,
                RateLimit = new NonDurableRateLimitOptions
                {
                    PermitLimit = 1,
                    Window = TimeSpan.FromSeconds(5)
                }
            }
        });

        // In tests, advance time to trigger delayed operations
        // simulatedTime.Advance(TimeSpan.FromSeconds(5));

        endpointConfiguration.UseTransport(new NonDurableTransport(new NonDurableTransportOptions(broker)));

        #endregion
    }

    void QueueOverride(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableTransport-QueueOverride

        var options = new NonDurableBrokerOptions
        {
            Send =
            {
                RateLimit = new NonDurableRateLimitOptions
                {
                    PermitLimit = 1,
                    Window = TimeSpan.FromSeconds(30)
                }
            }
        };

        // Override the default rate limit for a specific queue
        options.ForQueue("orders").Send.RateLimit = new NonDurableRateLimitOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(30)
        };

        var broker = new NonDurableBroker(options);
        endpointConfiguration.UseTransport(new NonDurableTransport(new NonDurableTransportOptions(broker)));

        #endregion
    }

    void DelayedDeliverySimulation(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableTransport-DelayedDeliverySimulation

        var simulatedTime = new FakeTimeProvider(DateTimeOffset.UtcNow);

        var broker = new NonDurableBroker(new NonDurableBrokerOptions
        {
            TimeProvider = simulatedTime,
            DelayedDelivery =
            {
                Mode = NonDurableSimulationMode.Delay,
                RateLimit = new NonDurableRateLimitOptions
                {
                    PermitLimit = 1,
                    Window = TimeSpan.FromSeconds(5)
                }
            }
        });

        endpointConfiguration.UseTransport(new NonDurableTransport(new NonDurableTransportOptions(broker)));

        #endregion
    }

    void ShutdownBehavior(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableTransport-ShutdownBehavior

        var transport = new NonDurableTransport(new NonDurableTransportOptions
        {
            ShutdownBehavior = NonDurableTransportShutdownBehavior.ShutdownAfterHandlerExit
        });

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
