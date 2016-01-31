﻿using NServiceBus.Logging;
using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Satellites;
using NServiceBus.Transports;
using NServiceBus.Transports.Msmq;
using NServiceBus.Unicast;

#region satellite
// Implements an advanced satellite. Allows to receive messages on a different transport.
class MsmqReceiver : IAdvancedSatellite
{
    Configure configure;
    CriticalError criticalError;
    // Since this endpoint's transport is usingSqlServer, the IPublishMessages will be using the SqlTransport to publish messages
    IPublishMessages publisher;
    static ILog logger = LogManager.GetLogger<MsmqReceiver>();
    public bool Disabled { get { return false; } }

    public MsmqReceiver(Configure configure, CriticalError criticalError, IPublishMessages publisher)
    {
        this.configure = configure;
        this.criticalError = criticalError;
        this.publisher = publisher;
    }

    // Since we want to listen to the events published by MSMQ, we are newing up MsmqDequeueStrategy and setting the
    // input queue to the queue which will be receiving all the events from the MSMQ publishers.
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


    // Will get invoked, whenever a new event is published by the Msmq publishers and when they notify the bridge. 
    // The bridge is a MSMQ and the publishers have an entry for this queue in their subscription storage.
    public bool Handle(TransportMessage message)
    {
        Type[] eventTypes = { Type.GetType(message.Headers["NServiceBus.EnclosedMessageTypes"]) };

        string msmqId = message.Headers["NServiceBus.MessageId"];
            
        // Set the Id to a deterministic guid, as Sql message Ids are Guids and Msmq message ids are guid\nnnn.
        // Newer versions of Nsb already return just a guid for the messageId. So, check to see if the Id is a valid Guid and if 
        // not, only then create a valid Guid. This check is important as it affects the retries if the message is rolled back.
        // If the Ids are different, then the FLR/SLR won't know its the same message.
        Guid newGuid;
        if (!Guid.TryParse(msmqId, out newGuid))
        {
            message.Headers["NServiceBus.MessageId"] = GuidBuilder.BuildDeterministicGuid(msmqId).ToString();
        }
        logger.Info("Forwarding message to all the SQL subscribers via a Publish");
        publisher.Publish(message, new PublishOptions(eventTypes.First()));
        return true;
    }

    // Address of the MSMQ that will be receiving all of the events from all of the MSMQ publishers.
    public Address InputAddress
    {
        get { return Address.Parse("SqlMsmqTransportBridge"); }
    }

    public void Start()
    {
    }

    public void Stop()
    {
    }
}
#endregion