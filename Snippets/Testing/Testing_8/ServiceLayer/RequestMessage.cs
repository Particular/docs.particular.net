﻿namespace Testing_8.ServiceLayer
{
    using NServiceBus;

    public class RequestMessage :
        IMessage
    {
        public string String { get; set; }
    }
}