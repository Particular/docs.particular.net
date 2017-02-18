using System;

class SecurityHelpers
{

    void Base64CSharp()
    {
        #region Base64-CSharp

        using (var rijndael = System.Security.Cryptography.Rijndael.Create())
        {
            rijndael.GenerateKey();
            var key = Convert.ToBase64String(rijndael.Key);
            Console.WriteLine(key);
        }

        #endregion

        #region Hex-CSharp

        using (var rijndael = System.Security.Cryptography.Rijndael.Create())
        {
            rijndael.GenerateKey();
            var key = BitConverter.ToString(rijndael.Key)
                .Replace("-", string.Empty)
                .ToLowerInvariant();
            Console.WriteLine(key);
        }

        #endregion
    }

}