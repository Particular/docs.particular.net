using System;
using System.IO;


class StreamWrapper : Stream
{
    Stream stream;
    int length;

    public StreamWrapper(Stream stream, int length)
    {
        this.stream = stream;
        this.length = length;
    }

    public override void Flush()
    {
        stream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return stream.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override bool CanRead
    {
        get { return stream.CanRead; }
    }

    public override bool CanSeek
    {
        get { return false; }
    }

    public override bool CanWrite
    {
        get { return stream.CanWrite; }
    }

    public override long Length
    {
        get { return length; }
    }

    public override long Position
    {
        get { return stream.Position; }
        set
        {
            if (value == 0)
            {
                return;
            }
            throw new NotImplementedException();
        }
    }
}
