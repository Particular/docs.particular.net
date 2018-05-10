using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using NServiceBus;
using NServiceBus.Attachments.FileShare;
using NServiceBus.Attachments.FileShare.Testing;
using NServiceBus.Testing;
using Xunit;

class TestingIncoming
{
    public void IncomingAttachment()
    {
        #region InjectAttachmentsInstance

        var context = new TestableMessageHandlerContext();
        var mockMessageAttachments = new MyMessageAttachments();
        context.InjectAttachmentsInstance(mockMessageAttachments);

        #endregion
    }

    #region CustomMockMessageAttachments

    public class CustomMockMessageAttachments : MockMessageAttachments
    {
        public override Task<AttachmentBytes> GetBytes(CancellationToken cancellation = default)
        {
            GetBytesWasCalled = true;
            return Task.FromResult(new AttachmentBytes(new byte[] {5}));
        }

        public bool GetBytesWasCalled { get; private set; }
    }

    #endregion

    #region TestIncomingHandler

    public class Handler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var attachment = context.Attachments();
            var bytes = await attachment.GetBytes();
        }
    }

    #endregion

    #region TestIncoming

    [Fact]
    public async Task TestIncomingAttachment()
    {
        //Arrange
        var context = new TestableMessageHandlerContext();
        var handler = new Handler();
        var mockMessageAttachments = new CustomMockMessageAttachments();
        context.InjectAttachmentsInstance(mockMessageAttachments);

        //Act
        await handler.Handle(new MyMessage(), context);

        //Assert
        Assert.True(mockMessageAttachments.GetBytesWasCalled);
    }

    #endregion
}

class MyMessageAttachments : IMessageAttachments
{
    public AttachmentStream GetStream()
    {
        throw new NotImplementedException();
    }

    public AttachmentStream GetStream(string name)
    {
        throw new NotImplementedException();
    }

    public AttachmentStream GetStreamForMessage(string messageId)
    {
        throw new NotImplementedException();
    }

    public AttachmentStream GetStreamForMessage(string messageId, string name)
    {
        throw new NotImplementedException();
    }

    public Task CopyTo(string name, Stream target, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task CopyTo(Stream target, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task ProcessStream(string name, Func<AttachmentStream, Task> action, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task ProcessStream(Func<AttachmentStream, Task> action, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreams(Func<string, AttachmentStream, Task> action, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<AttachmentBytes> GetBytes(CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<AttachmentBytes> GetBytes(string name, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task CopyToForMessage(string messageId, string name, Stream target, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task CopyToForMessage(string messageId, Stream target, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreamForMessage(string messageId, string name, Func<AttachmentStream, Task> action, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreamForMessage(string messageId, Func<AttachmentStream, Task> action, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task ProcessStreamsForMessage(string messageId, Func<string, AttachmentStream, Task> action, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<AttachmentBytes> GetBytesForMessage(string messageId, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<AttachmentBytes> GetBytesForMessage(string messageId, string name, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}