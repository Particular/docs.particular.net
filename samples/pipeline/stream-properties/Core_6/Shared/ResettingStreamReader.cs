using System.IO;
using System.Text;

#region stream-reader-helper

public class ResettingStreamReader : StreamReader
{
    public ResettingStreamReader(Stream stream) : base(
        stream: stream,
        encoding: Encoding.UTF8,
        detectEncodingFromByteOrderMarks: true,
        bufferSize: 1024,
        leaveOpen: true)
    {
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        BaseStream.Position = 0;
    }
}

#endregion
