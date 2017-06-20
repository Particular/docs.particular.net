using System;
using System.Security.Cryptography;
using System.Text;

static class GuidBuilder
{
    public static Guid BuildDeterministicGuid(string msmqMessageId)
    {
        // use MD5 hash to get a 16-byte hash of the string
        using (var provider = new MD5CryptoServiceProvider())
        {
            var inputBytes = Encoding.Default.GetBytes(msmqMessageId);
            var hashBytes = provider.ComputeHash(inputBytes);
            // generate a guid from the hash:
            return new Guid(hashBytes);
        }
    }
}