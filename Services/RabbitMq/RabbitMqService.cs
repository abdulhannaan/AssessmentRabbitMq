using Azure.Core;
using InboundApi.Helpers;
using InboundApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InboundApi.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _rabbitMqConnection;
        private IModel _channel;
        private RabbitMqSettings _rabbitMqSettings;

        public RabbitMqService(IConnection rabbitMqConnection, RabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _rabbitMqSettings = rabbitMqSettings;
        }
        public async Task SendMessageToQueueAsync(MyRequestModel request)
        {
            EnsureConnection();
            CreateChannel();
            EnsureQueueExists();

            var message = JsonSerializer.Serialize(request); 
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: string.Empty, routingKey: _rabbitMqSettings.QueueName, basicProperties: null, body: body);
        }

        private void EnsureConnection()
        {
            if (_rabbitMqConnection == null || !_rabbitMqConnection.IsOpen)
            {
                throw new InvalidOperationException(ResponseMessage.RabbitMQConnectionNotEstablished);
            }
        }

        private void CreateChannel()
        {
            if (_channel == null || !_channel.IsOpen)
            {
                _channel = _rabbitMqConnection.CreateModel();
            }
        }

        private void EnsureQueueExists()
        {
            _channel.QueueDeclare(queue: _rabbitMqSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void StartConsuming(EventHandler<BasicDeliverEventArgs> onMessageReceived)
        {
            EnsureConnection();
            CreateChannel();
            EnsureQueueExists();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += onMessageReceived; 
            _channel.BasicConsume(queue: _rabbitMqSettings.QueueName, autoAck: false, consumer: consumer);
        }

        //~RabbitMqService()
        //{
        //    _channel?.Dispose();
        //}
    }
}
