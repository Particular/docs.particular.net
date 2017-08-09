namespace SqsAll
{
    using System;
    using System.Linq;

    #region sqs-queue-name-helper
    public static class QueueNameHelper
    {
        public static string GetSqsQueueName(string destination, string queueNamePrefix = null, bool preTruncateQueueNames = false)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException(nameof(destination));
            }


            var s = queueNamePrefix + destination;

            if (preTruncateQueueNames && s.Length > 80)
            {
                if (string.IsNullOrWhiteSpace(queueNamePrefix))
                {
                    throw new ArgumentNullException(nameof(queueNamePrefix));
                }

                var charsToTake = 80 - queueNamePrefix.Length;
                s = queueNamePrefix +
                    new string(s.Reverse().Take(charsToTake).Reverse().ToArray());
            }

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
                throw new Exception(
                    $"Address {destination} with configured prefix {queueNamePrefix} is longer than 80 characters and therefore cannot be used to create an SQS queue. Use a shorter queue name.");
            }

            return s;
        }
    }
    #endregion
}