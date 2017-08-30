namespace Wcf_2.Object
{
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