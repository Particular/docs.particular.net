namespace Wcf_2.Int
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