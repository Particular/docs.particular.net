using System;
using NServiceBus;

namespace Messages
{
    #region Message
    public class Command : IMessage
    {
        public int Id { get; set; }
    }
    #endregion
}