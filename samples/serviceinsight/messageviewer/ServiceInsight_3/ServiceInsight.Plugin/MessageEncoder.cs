using System.Linq;

namespace ServiceInsight.CustomViewer.Plugin;

class MessageEncoder : IMessageEncoder
{
    public byte[] Decrypt(byte[] cipherText)
        => [.. cipherText.Reverse()];

    public byte[] Encrypt(byte[] plainText)
        => [.. plainText.Reverse()];
}
