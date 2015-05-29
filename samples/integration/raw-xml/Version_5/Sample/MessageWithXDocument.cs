using System.Xml.Linq;
using NServiceBus;
// ReSharper disable InconsistentNaming

namespace Messages
{
    #region MessageWithXDocument
    public class MessageWithXDocument : IMessage
    {
        public XDocument codes { get; set; }
    }
    #endregion
}
