namespace Wcf_2.Int
{
    using NServiceBus;

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