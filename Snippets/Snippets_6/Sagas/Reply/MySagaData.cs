using System;
using NServiceBus.Saga;

namespace Snippets6.Sagas.Reply
{
    public class MySagaData : ContainSagaData
    {
        public string SomeID { get; set; }

        public bool Message2Arrived { get; set; }
    }
}