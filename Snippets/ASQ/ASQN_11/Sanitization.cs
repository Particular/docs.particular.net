// ReSharper disable SuggestVarOrType_Elsewhere

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NServiceBus;

[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Sanitization
{
    void CustomSanitization(EndpointConfiguration endpointConfiguration)
    {
        #region azure-storage-queue-sanitization

        var transport = new AzureStorageQueueTransport("connection string")
        {
            QueueNameSanitizer = queueName => queueName.Replace('.', '-')
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void SanitizeWithMd5BackwardsCompatible(EndpointConfiguration endpointConfiguration)
    {
        #region azure-storage-queue-backwards-compatible-sanitization-with-md5

        var transport = new AzureStorageQueueTransport("connection string")
        {
            QueueNameSanitizer = BackwardsCompatibleQueueNameSanitizer.WithMd5Shortener
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    #region azure-storage-queue-backwards-compatible-sanitization

    public static class BackwardsCompatibleQueueNameSanitizer
    {
        public static string WithMd5Shortener(string queueName)
        {
            return Sanitize(queueName, useMd5Hashing: true);
        }

        public static string WithSha1Shortener(string queueName)
        {
            return Sanitize(queueName, useMd5Hashing: false);
        }

        static string Sanitize(string queueName, bool useMd5Hashing = true)
        {
            var queueNameInLowerCase = queueName.ToLowerInvariant();
            return ShortenQueueNameIfNecessary(SanitizeQueueName(queueNameInLowerCase), useMd5Hashing);
        }

        static string ShortenQueueNameIfNecessary(string sanitizedQueueName, bool useMd5Hashing)
        {
            if (sanitizedQueueName.Length <= 63)
            {
                return sanitizedQueueName;
            }

            var shortenedName = useMd5Hashing ? ShortenWithMd5(sanitizedQueueName) : ShortenWithSha1(sanitizedQueueName);

            return $"{sanitizedQueueName.Substring(0, 63 - shortenedName.Length - 1).Trim('-')}-{shortenedName}";
        }

        static string SanitizeQueueName(string queueName)
        {
            // this can lead to multiple '-' occurrences in a row
            var sanitized = invalidCharacters.Replace(queueName, "-");
            return multipleDashes.Replace(sanitized, "-");
        }

        static string ShortenWithMd5(string test)
        {
            //use MD5 hash to get a 16-byte hash of the string
            using (var provider = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(test);
                var hashBytes = provider.ComputeHash(inputBytes);
                //generate a GUID from the hash:
                return new Guid(hashBytes).ToString();
            }
        }

        static string ShortenWithSha1(string queueName)
        {
            using (var provider = SHA1.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(queueName);
                var hashBytes = provider.ComputeHash(inputBytes);

                return ToChars(hashBytes);
            }
        }

        static string ToChars(byte[] hashBytes)
        {
            var chars = new char[hashBytes.Length * 2];
            for (var i = 0; i < chars.Length; i += 2)
            {
                var byteIndex = i / 2;
                chars[i] = HexToChar((byte)(hashBytes[byteIndex] >> 4));
                chars[i + 1] = HexToChar(hashBytes[byteIndex]);
            }

            return new string(chars);
        }

        static char HexToChar(byte a)
        {
            a &= 15;
            return a > 9 ? (char)(a - 10 + 97) : (char)(a + 48);
        }

        static Regex invalidCharacters = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);
        static Regex multipleDashes = new Regex(@"\-+", RegexOptions.Compiled);
    }
    #endregion
}