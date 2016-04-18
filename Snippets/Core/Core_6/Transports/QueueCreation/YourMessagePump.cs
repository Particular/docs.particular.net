namespace Core6.Transports.QueueCreation
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Transports;

    class YourMessagePump : IPushMessages
    {
        public Task Init(Func<PushContext, Task> pipe, CriticalError criticalError, PushSettings settings)
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