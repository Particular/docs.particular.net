using System.Text;

static class SharedKeys
{
    public static byte[] SigningKey { get; }

    static SharedKeys()
    {
        SigningKey = Encoding.UTF8.GetBytes("This is not a secure encryption key, but just a sample. For production, do not create encryption/signing keys using this method.");
    }
}
