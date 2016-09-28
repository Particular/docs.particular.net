using NServiceBus;

namespace Wcf1.Enum
{
    class Usage
    {
        #region WcfEnumCallback

        class MyService :
            WcfService<Message, Status>
        {
        }

        #endregion
    }
}