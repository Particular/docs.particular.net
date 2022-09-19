namespace Shared
{
    public interface IMessageEncoder
    {
        byte[] Encrypt(byte[] plainText);
        byte[] Decrypt(byte[] cipherText);
    }
}