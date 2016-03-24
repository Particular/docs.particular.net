namespace Receiver
{
    using NServiceBus.Pipeline;

    public class PipelineExecution
    {
        static PipelineExecution()
        {
            Instance = new PipelineExecution();
        }

        public static PipelineExecution Instance { get; }

        public IIncomingPhysicalMessageContext CurrentContext { get; set; }
    }
}