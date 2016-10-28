using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Faults;
using NServiceBus.Logging;
using NServiceBus.Transports;
using NServiceBus.Unicast;

public class CustomFaultManager : IManageMessageFailures
{
    ISendMessages sender;
    MessageForwardingInCaseOfFaultConfig config;
    static ILog Log = LogManager.GetLogger(typeof(CustomFaultManager));
    Address localAddress;

    public CustomFaultManager(ISendMessages sender, IProvideConfiguration<MessageForwardingInCaseOfFaultConfig> config)
    {
        this.sender = sender;
        this.config = config.GetConfiguration();
    }

    #region MoveToErrorQueue
    public void SerializationFailedForMessage(TransportMessage message, Exception e)
    {
        SendToErrorQueue(message);
    }

    public void ProcessingAlwaysFailsForMessage(TransportMessage message, Exception e)
    {
        if (e is MyCustomException)
        {
            //Ignore the exception, beware here be dragons!
            Log.WarnFormat("MyCustomException was thrown. Ignoring the error for message Id {0}.", message.Id);
        }
        else
        {
            //Check if you have performed enough retries, ultimately send to error queue
            SendToErrorQueue(message);
        }
    }
    #endregion

    void SendToErrorQueue(TransportMessage message)
    {
        message.TimeToBeReceived = TimeSpan.MaxValue;
        sender.Send(message, new SendOptions(config.ErrorQueue));
        Log.WarnFormat("Message {0} was moved to the error queue.", message.Id);
    }

    public void Init(Address address)
    {
        localAddress = address;
    }
}