using System;
using NServiceBus;

namespace Shared
{
    #region OrderPlaced

    public class OrderPlaced : IEvent
    {
        public Guid OrderId { get; set; }
    }

    #endregion
}