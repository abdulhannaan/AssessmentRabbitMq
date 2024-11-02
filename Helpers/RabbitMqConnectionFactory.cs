using InboundApi.Models;
using RabbitMQ.Client;

public class RabbitMqConnectionFactory
{
    private readonly RabbitMqSettings _rabbitMqSettings;

    public RabbitMqConnectionFactory(RabbitMqSettings rabbitMqSettings)
    {
        _rabbitMqSettings = rabbitMqSettings;
    }

    public IConnection CreateRabbitMqConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.HostName,
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password
        };

        return factory.CreateConnection();
    }
}
