using System;
using System.Text;

static class Extensions
{
    public static string Truncate(this string str, int maxLength)
    {
        if (str.Length > maxLength)
        {
            return $"{str.Substring(0, maxLength)}...";
        }
        return str;
    }

    public static string DecodeFromKey(this string encodedKey)
    {
        var base64 = encodedKey.Replace('_', '/');
        var bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }
}