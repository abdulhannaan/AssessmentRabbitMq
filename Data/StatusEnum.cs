namespace InboundApi.Data
{
    enum StatusEnum
    {
        Received,
        SuccessfullyProcessed,
        SuccessfullyConsumed,
        FileWritten,
        ErrorConsuming,
        ErrorProcessing
    }
}
