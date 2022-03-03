namespace Core7.Sagas.Timeouts
{
    using NServiceBus;
    using System;

    public class MySagaData :
        ContainSagaData
    {
        public string SomeId { get; set; }

        public bool Message2Arrived { get; set; }
    }
}