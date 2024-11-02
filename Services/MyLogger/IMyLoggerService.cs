using InboundApi.Models;

namespace InboundApi.Services
{
    public interface IMyLoggerService
    {
        Task LogAsync(MyRequestModel request, string status);
    }
}
