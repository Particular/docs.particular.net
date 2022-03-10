namespace Testing_8.Saga
{
    using System;
    using NServiceBus;

    public class MySagaData :
        ContainSagaData
    {
        public string MyId { get; set; }
    }
}