using System.Xml.Linq;
using NServiceBus;
// ReSharper disable InconsistentNaming

namespace Messages
{
    #region MessageWithXElement
    public class MessageWithXElement : IMessage
    {
        public XElement codes { get; set; }
    }
    #endregion
}
