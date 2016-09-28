using NServiceBus;

namespace Wcf1.Int
{
    class Usage
    {
        #region WcfIntCallback

        class MyService :
            WcfService<Message, int>
        {
        }

        #endregion
    }
}