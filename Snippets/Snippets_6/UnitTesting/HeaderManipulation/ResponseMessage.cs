﻿namespace Snippets6.UnitTesting.HeaderManipulation
{
    using NServiceBus;

    class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }
}