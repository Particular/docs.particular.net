namespace Core6.Errors.SecondLevel.CustomPolicies
{
    using NServiceBus;
    using NServiceBus.Transports;

    #region ErrorsHeadersHelper

    static class ErrorsHeadersHelper
    {

        internal static int NumberOfRetries(this IncomingMessage incomingMessage)
        {
            string value;
            if (incomingMessage.Headers.TryGetValue(Headers.Retries, out value))
            {
                return int.Parse(value);
            }
            return 0;
        }

        internal static string ExceptionType(this IncomingMessage incomingMessage)
        {
            return incomingMessage.Headers["NServiceBus.ExceptionInfo.ExceptionType"];
        }

    }

    #endregion
}