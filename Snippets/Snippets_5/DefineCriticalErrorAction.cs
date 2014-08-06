using NServiceBus;

// ReSharper disable once ConvertToLambdaExpression
public class DefineCriticalErrorAction
{
    public void Simple()
    {
        #region DefineCriticalErrorActionV5

        var configure = Configure.With(builder =>
        {
            builder.DefineCriticalErrorAction((s, exception) =>
            {
                // custom exception handling
            });
        });

        #endregion
    }

}