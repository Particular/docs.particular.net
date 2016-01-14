using System;
using System.Text;

public static class Extensions
{
    public static string Truncate(this string str, int maxLength)
    {
        if (str.Length > maxLength)
        {
            return str.Substring(0,maxLength) + "...";
        }
        return str;
    }
    
    public static string DecodeFromKey(this string encodedKey)
    {
        string base64 = encodedKey.Replace('_', '/');
        byte[] bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }
}