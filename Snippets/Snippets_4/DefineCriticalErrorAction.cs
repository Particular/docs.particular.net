using NServiceBus;

public class DefineCriticalErrorAction
{
    public void Simple()
    {
        #region DefineCriticalErrorActionV4

        var configure = Configure.With()
            .DefineCriticalErrorAction((s, exception) =>
            {
                // custom exception handling
            });

        #endregion
    }

}