using NServiceBus;

public class DefineCriticalErrorAction
{
    public void Simple()
    {
        // start code DefineCriticalErrorActionV4

        var configure = Configure.With()
            .DefineCriticalErrorAction((s, exception) =>
            {
                // custom exception handling
            });

        // end code DefineCriticalErrorActionV4
    }

}