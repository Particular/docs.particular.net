using NServiceBus;


public class SendOnly
{
    public void Simple()
    {
        #region SendOnlyV5

        var bus = Configure.With(
            b => b.SendOnly())
            .CreateBus();

        #endregion
    }

}