namespace Rabbit_3.UpgradeGuides._3to4
{
    using System;
    using NServiceBus;

    class Upgrade
    {
        void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
        {
            #region 3to4rabbitmq-config-callbackreceiver-thread-count

            var transport = busConfiguration.UseTransport<RabbitMQTransport>();
            transport.CallbackReceiverMaxConcurrency(10);

            #endregion
        }

        void UseDirectRoutingTopology(BusConfiguration busConfiguration)
        {
            #region 3to4rabbitmq-config-usedirectroutingtopology

            var transport = busConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseDirectRoutingTopology(MyRoutingKeyConvention, (address, eventType) => "MyTopic");

            #endregion
        }

        string MyRoutingKeyConvention(Type type)
        {
            throw new NotImplementedException();
        }
    }
}