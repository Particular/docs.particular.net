namespace Core9.Sagas.Timeouts
{
    using System;
    using NServiceBus;

    public class MySagaData :
        ContainSagaData
    {
        public string SomeId { get; set; }

        public bool Message2Arrived { get; set; }
    }
}