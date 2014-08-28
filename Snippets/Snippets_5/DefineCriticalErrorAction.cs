using NServiceBus;

// ReSharper disable once ConvertToLambdaExpression
public class DefineCriticalErrorAction
{
    public void Simple()
    {
        #region DefineCriticalErrorActionV5

        var configuration = new BusConfiguration();

        configuration.DefineCriticalErrorAction((s, exception) =>
            {
                // custom exception handling
            });


        #endregion
    }

}