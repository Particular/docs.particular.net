namespace SqsAll
{
    using System;

    #region sqs-queue-name-helper
    public static class QueueNameHelper
    {
        public static string GetSqsQueueName(string destination, string queueNamePrefix = null)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var s = queueNamePrefix + destination;

            // SQS queue names can only have alphanumeric characters, hyphens and underscores.
            // Any other characters will be replaced with a hyphen.
            for (var i = 0; i < s.Length; ++i)
            {
                var c = s[i];
                if (!char.IsLetterOrDigit(c)
                    && c != '-'
                    && c != '_')
                {
                    s = s.Replace(c, '-');
                }
            }

            if (s.Length > 80)
            {
                throw new ArgumentException(
                    $"Address {destination} with configured prefix {queueNamePrefix} is longer than 80 characters and therefore cannot be used to create an SQS queue. Use a shorter endpoint name or a shorter queue name prefix.");
            }

            return s;
        }
    }
    #endregion
}