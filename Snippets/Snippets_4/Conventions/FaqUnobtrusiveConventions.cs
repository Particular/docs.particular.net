using NServiceBus;

class FaqUnobtrusiveConventions
{
    public void Init()
    {
        Configure.With()
            .DefaultBuilder()
            #region UnobtrusiveConventionsFaqError

            .DefiningMessagesAs(t => 
                t.Namespace != null && 
                t.Namespace.EndsWith("Messages"));

        #endregion UnobtrusiveConventionsFaqError

        Configure.With()
            .DefaultBuilder()
            #region UnobtrusiveConventionsFaqFix

            .DefiningMessagesAs(t =>
                t.Namespace != null &&
                t.Namespace.StartsWith("MyCompany") &&
                t.Namespace.EndsWith("Messages"));

        #endregion UnobtrusiveConventionsFaqFix

    }
}