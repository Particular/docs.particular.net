using NServiceBus;
using System;

namespace Shared
{
    public class ResponseMessage
        : IMessage
    {
        public Guid Id { get; set; }
        public string Data { get; set; }
    }
}