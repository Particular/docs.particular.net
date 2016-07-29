namespace Core6.Transports.QueueCreation
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Transport;

    class YourMessagePump :
        IPushMessages
    {

        public Task Init(Func<MessageContext, Task> onMessage, Func<ErrorContext, Task<ErrorHandleResult>> onError, CriticalError criticalError, PushSettings settings)
        {
            throw new NotImplementedException();
        }

        public void Start(PushRuntimeSettings limitations)
        {
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}