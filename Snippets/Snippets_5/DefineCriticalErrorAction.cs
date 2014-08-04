using NServiceBus;

// ReSharper disable once ConvertToLambdaExpression
public class DefineCriticalErrorAction
{
    public void Simple()
    {
        // start code DefineCriticalErrorActionV5

        var configure = Configure.With(builder =>
        {
            builder.DefineCriticalErrorAction((s, exception) =>
            {
                // custom exception handling
            });
        });

        // end code DefineCriticalErrorActionV
    }

}