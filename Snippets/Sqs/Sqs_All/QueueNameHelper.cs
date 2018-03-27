namespace SqsAll
{
    using System;
    using System.Text;

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

            if (s.Length > 80)
            {
                throw new ArgumentException(
                    $"Address {destination} with configured prefix {queueNamePrefix} is longer than 80 characters and therefore cannot be used to create an SQS queue. Use a shorter endpoint name or a shorter queue name prefix.");
            }

            // SQS queue names can only have alphanumeric characters, hyphens and underscores.
            // Any other characters will be replaced with a hyphen.
            var skipCharacters = s.EndsWith(".fifo") ? 5 : 0;
            var queueNameBuilder = new StringBuilder(s);

            for (var i = 0; i < queueNameBuilder.Length - skipCharacters; ++i)
            {
                var c = queueNameBuilder[i];
                if (!char.IsLetterOrDigit(c)
                    && c != '-'
                    && c != '_')
                {
                    queueNameBuilder[i] = '-';
                }
            }

            return queueNameBuilder.ToString();
        }
    }
    #endregion
}