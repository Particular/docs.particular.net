using System;
using System.Text;

static class Extensions
{
    public static string Base64Decode(this string encodedBody)
    {
        byte[] bytes = Convert.FromBase64String(encodedBody);
        return Encoding.UTF8.GetString(bytes);
    }
}