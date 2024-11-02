using InboundApi.Models;
using RabbitMQ.Client.Events;

namespace InboundApi.Services
{
    public interface IRabbitMqService
    {
        Task SendMessageToQueueAsync(MyRequestModel request);

        void StartConsuming(EventHandler<BasicDeliverEventArgs> onMessageReceived);
    }
}
