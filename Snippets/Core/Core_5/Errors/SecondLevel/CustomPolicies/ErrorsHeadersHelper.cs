namespace Core5.Errors.SecondLevel.CustomPolicy
{
    using NServiceBus;

    #region ErrorsHeadersHelper

    static class ErrorsHeadersHelper
    {
        internal static int NumberOfRetries(this TransportMessage transportMessage)
        {
            string value;
            var headers = transportMessage.Headers;
            if (headers.TryGetValue(Headers.Retries, out value))
            {
                return int.Parse(value);
            }
            return 0;
        }

        internal static string ExceptionType(this TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            return headers["NServiceBus.ExceptionInfo.ExceptionType"];
        }
    }

    #endregion
}
