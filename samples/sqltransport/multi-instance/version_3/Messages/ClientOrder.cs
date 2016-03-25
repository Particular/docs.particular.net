using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    #region MessageContract

    public class ClientOrder : IMessage
    {
        public Guid OrderId { get; set; }
    }

    public class ClientOrderAccepted : IMessage
    {
        public Guid OrderId { get; set; }
    }
    #endregion
}
