namespace SqsAll
{
    using System;

    static class EnvironmentHelper
    {
        public static string GetEnvironmentVariable(string variable)
        {
            var candidate = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User);

            if (string.IsNullOrWhiteSpace(candidate))
            {
                return Environment.GetEnvironmentVariable(variable);
            }

            return candidate;
        }
    }
}