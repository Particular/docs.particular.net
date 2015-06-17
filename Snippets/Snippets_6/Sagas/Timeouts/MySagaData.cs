using System;
using NServiceBus.Saga;

namespace Snippets6.Sagas.Timeouts
{
    public class MySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public string SomeID { get; set; }

        public bool Message2Arrived { get; set; }
    }
}