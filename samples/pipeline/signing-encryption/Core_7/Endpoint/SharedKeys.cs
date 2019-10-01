using System.Text;

static class SharedKeys
{
    public static byte[] SigningKey { get; }

    static SharedKeys()
    {
        SigningKey = Encoding.UTF8.GetBytes("This is not a secure encryption key, but this is just a sample. Seriously, this is not how you should be creating encryption/signing keys.");
    }
}