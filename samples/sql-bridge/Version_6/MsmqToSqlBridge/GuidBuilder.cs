using System;
using System.Security.Cryptography;
using System.Text;

namespace MsmqToSqlBridge
{
    static class GuidBuilder
    {
        public static Guid BuildDeterministicGuid(string msmqMessageId)
        {
            // use MD5 hash to get a 16-byte hash of the string
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                byte[] inputBytes = Encoding.Default.GetBytes(msmqMessageId);
                byte[] hashBytes = provider.ComputeHash(inputBytes);
                // generate a guid from the hash:
                return new Guid(hashBytes);
            }
        }
    }
}