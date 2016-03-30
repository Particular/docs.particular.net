namespace Rabbit_3.UpgradeGuides._3to4
{
    using System;
    using NServiceBus;

    class Upgrade
    {
        void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
        {
            #region 3to4rabbitmq-config-callbackreceiver-thread-count

            busConfiguration.UseTransport<RabbitMQTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }

        void UseDirectRoutingTopology(BusConfiguration busConfiguration)
        {
            #region 3to4rabbitmq-config-usedirectroutingtopology
            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseDirectRoutingTopology(MyRoutingKeyConvention, (address, eventType) => "MyTopic");

            #endregion
        }

        string MyRoutingKeyConvention(Type type)
        {
            throw new NotImplementedException();
        }
    }
}