namespace Wcf_3.Object
{
    using NServiceBus;

    class Usage
    {
        #region WcfObjectCallback

        class MyService :
            WcfService<Message, ResponseMessage>
        {
        }

        #endregion
    }
}