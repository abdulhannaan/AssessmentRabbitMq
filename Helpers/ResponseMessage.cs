namespace InboundApi.Helpers
{
    public static class ResponseMessage
    {
        public const string RabbitMQConnectionNotEstablished = "RabbitMQ connection is not established.";
        public const string StartedConsumingMessages = "Started consuming messages from RabbitMQ.";
        public const string ErrorConsumingRequest = "Error consuming request";
        public const string ErrorProcessingRequest = "Error processing request";
        public const string ErrorProcessingFile = "Error processing file";
        public const string FileWrittenSuccessfully = "File Written Successfully";
        public const string Error = "Error";

    }
}
