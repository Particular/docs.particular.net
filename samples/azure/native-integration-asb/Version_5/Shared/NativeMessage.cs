using System;
using NServiceBus;

namespace Shared
{

    #region NativeMessage

    public class NativeMessage : IMessage
    {
        public string Content { get; set; }
        public DateTime SendOnUtc { get; set; }
    }

    #endregion
}