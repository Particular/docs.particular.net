namespace Common
{
    #region SHA1DeterministicGuid

    using System;
    using System.Security.Cryptography;
    using System.Text;

    static class Sha1DeterministicGuid
    {
        public static Guid Create(params object[] data)
        {
            var inputBytes = Encoding.UTF8.GetBytes(string.Concat(data));

            using (var provider = new SHA1Managed())
            {
                var guidBytes = new byte[16];

                var hashBytes = provider.ComputeHash(inputBytes);

                Array.ConstrainedCopy(hashBytes, 0, guidBytes, 0, 16);

                return new Guid(guidBytes);
            }
        }
    }
    #endregion
}
