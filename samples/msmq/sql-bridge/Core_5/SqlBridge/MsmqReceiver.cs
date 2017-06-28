using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using NServiceBus;
using NServiceBus.Satellites;
using NServiceBus.Transports;
using NServiceBus.Transports.Msmq;
using NServiceBus.Unicast;

#region satellite
// Implements an advanced satellite.
// Allows to receive messages on a different transport.
class MsmqReceiver :
    IAdvancedSatellite
{
    Configure configure;
    CriticalError criticalError;
    // Since this endpoint's transport is usingSqlServer, the IPublishMessages
    // will be using the SqlTransport to publish messages
    IPublishMessages publisher;
    static ILog log = LogManager.GetLogger<MsmqReceiver>();
    public bool Disabled => false;

    public MsmqReceiver(Configure configure, CriticalError criticalError, IPublishMessages publisher)
    {
        this.configure = configure;
        this.criticalError = criticalError;
        this.publisher = publisher;
    }

    // Since listening to the events published by MSMQ, a new
    // MsmqDequeueStrategy is constructed and setting the input queue to the
    // queue which will be receiving all the events from the MSMQ publishers.
    public Action<NServiceBus.Unicast.Transport.TransportReceiver> GetReceiverCustomization()
    {
        return tr =>
        {
            tr.Receiver = new MsmqDequeueStrategy(configure, criticalError, new MsmqUnitOfWork())
            {
                ErrorQueue = Address.Parse(ConfigErrorQueue.errorQueue)
            };
        };
    }


    // Will get invoked, whenever a new event is published by the Msmq publishers
    // and when they notify the bridge. The bridge is a MSMQ and the publishers
    // have an entry for this queue in their subscription storage.
    public bool Handle(TransportMessage msmqMessage)
    {
        var eventTypes = ExtractEventTypes(msmqMessage.Headers);
        var sqlMessage = TranslateToSqlTransportMessage(msmqMessage);

        log.Info("Forwarding message to all the SQL subscribers via a Publish");

        publisher.Publish(sqlMessage, new PublishOptions(eventTypes.First()));
        return true;
    }

    static Type[] ExtractEventTypes(Dictionary<string, string> headers)
    {
        return new []
        {
            Type.GetType(headers["NServiceBus.EnclosedMessageTypes"])
        };
    }

    static TransportMessage TranslateToSqlTransportMessage(TransportMessage msmqMessage)
    {
        var headers = msmqMessage.Headers;
        var msmqId = headers["NServiceBus.MessageId"];
        var sqlId = msmqId;

        // Set the Id to a deterministic guid, as Sql message Ids are Guids and
        // Msmq message ids are guid\nnnn. Newer versions of NServiceBus already
        // return just a guid for the messageId. So, check to see if the Id is
        // a valid Guid and if not, only then create a valid Guid. This check
        // is important as it affects the retries if the message is rolled back.
        // If the Ids are different, then the recoverability won't know its the same message.
        Guid msmqGuid;

        if (!Guid.TryParse(msmqId, out msmqGuid))
        {
            sqlId = GuidBuilder.BuildDeterministicGuid(msmqId).ToString();
            headers["NServiceBus.MessageId"] = sqlId;
        }

        return new TransportMessage(sqlId, headers)
        {
            Body = msmqMessage.Body,
            TimeToBeReceived = (msmqMessage.TimeToBeReceived < Message.InfiniteTimeout) ? msmqMessage.TimeToBeReceived : TimeSpan.MaxValue
        };
    }

    // Address of the MSMQ that will be receiving all of the events from
    // all of the MSMQ publishers.
    public Address InputAddress => Address.Parse("SqlMsmqTransportBridge");

    public void Start()
    {
    }

    public void Stop()
    {
    }
}
#endregion