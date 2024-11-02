using InboundApi.Data.Repositories;
using InboundApi.Data;
using InboundApi.Models;
using InboundApi.Services;

public class MyLoggerService : IMyLoggerService
{
    private readonly IMyLoggerRepository _myLoggerRepository;

    public MyLoggerService(IMyLoggerRepository myLoggerRepository)
    {
        _myLoggerRepository = myLoggerRepository;
    }

    public async Task LogAsync(MyRequestModel request, string status)
    {
        var logEntry = new MyLogger
        {
            LogDate = DateTime.UtcNow,
            Originator = request.Originator,
            FileName = request.FileName,
            Status = status
        };

        await _myLoggerRepository.AddAsync(logEntry);
    }
}
