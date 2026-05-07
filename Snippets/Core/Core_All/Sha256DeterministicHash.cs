namespace Common
{
    #region SHA256DeterministicGuid

    using System;
    using System.Security.Cryptography;
    using System.Text;

    static class Sha256DeterministicGuid
    {
        public static Guid Create(params object[] data)
        {
            var inputBytes = Encoding.UTF8.GetBytes(string.Concat(data));

            using (var sha256 = SHA256.Create())
            {
                var guidBytes = new byte[16];

                var hashBytes = sha256.ComputeHash(inputBytes);

                Array.ConstrainedCopy(hashBytes, 0, guidBytes, 0, 16);

                return new Guid(guidBytes);
            }
        }
    }
    #endregion
}