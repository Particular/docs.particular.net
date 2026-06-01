namespace Common
{
    #region XxHash128DeterministicGuid

    using System;
    using System.IO.Hashing;
    using System.Text;

    static class XxHash128DeterministicGuid
    {
        public static Guid Create(params string[] values)
        {
            var encoding = Encoding.UTF8;
            var hash = new XxHash128();

            foreach (var value in values)
            {
                var bytes = encoding.GetBytes(value);
                var lengthPrefix = BitConverter.GetBytes(bytes.Length);
                hash.Append(lengthPrefix);
                hash.Append(bytes);
            }

            var hashBytes = hash.GetCurrentHash();

            // UUID version 8
            hashBytes[6] = (byte)((hashBytes[6] & 0x0F) | 0x80);
            // RFC 4122 / RFC 9562 variant
            hashBytes[8] = (byte)((hashBytes[8] & 0x3F) | 0x80);

            // XxHash128 outputs big-endian bytes but Guid(byte[]) expects little-endian on .NET Framework.
            // Swap the first three groups to match the Guid binary layout.
            SwapBytes(hashBytes, 0, 3);
            SwapBytes(hashBytes, 1, 2);
            SwapBytes(hashBytes, 4, 5);
            SwapBytes(hashBytes, 6, 7);

            return new Guid(hashBytes);
        }

        static void SwapBytes(byte[] bytes, int a, int b)
        {
            (bytes[a], bytes[b]) = (bytes[b], bytes[a]);
        }
    }
    #endregion
}