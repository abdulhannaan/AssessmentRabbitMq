namespace InboundApi.Helpers
{
    public static class ResponseMessage
    {
        public const string RabbitMQConnectionNotEstablished = "RabbitMQ connection is not established.";
        public const string StartedConsumingMessages = "Started consuming messages from RabbitMQ.";
        public const string ErrorProcessingMessage = "Error processing message";
        public const string ErrorProcessingFile = "Error processing file";
        public const string FileWrittenSuccessfully = "File Written Successfully";

    }
}
