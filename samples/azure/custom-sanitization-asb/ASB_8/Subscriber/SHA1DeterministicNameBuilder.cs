using System.Linq;
using System.Security.Cryptography;
using System.Text;

#region SHA1DeterministicNameBuilder

static class SHA1DeterministicNameBuilder
{
    public static string Build(string input)
    {
        using (var provider = new SHA1CryptoServiceProvider())
        {
            var inputBytes = Encoding.Default.GetBytes(input);
            var hashBytes = provider.ComputeHash(inputBytes);

            var hashBuilder = new StringBuilder(string.Join("", hashBytes.Select(x => x.ToString("x2"))));
            foreach (var delimeterIndex in new[]
            {
                5,
                11,
                17,
                23,
                29,
                35,
                41
            })
            {
                hashBuilder.Insert(delimeterIndex, "-");
            }
            return hashBuilder.ToString();
        }
    }
}

#endregion
