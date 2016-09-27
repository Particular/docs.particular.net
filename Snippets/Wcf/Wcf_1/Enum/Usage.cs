using NServiceBus;

namespace Wcf1.Enum
{
    class Usage
    {
        #region WcfEnum

        class MyService :
            WcfService<Message, Status>
        {
        }

        #endregion
    }
}
