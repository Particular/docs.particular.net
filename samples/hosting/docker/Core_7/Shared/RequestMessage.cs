using NServiceBus;
using System;

namespace Shared
{
    public class RequestMessage
        : IMessage
    {
        public Guid Id { get; set; }
        public string Data { get; set; }
    }
}