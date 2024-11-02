using InboundApi.Helpers;
using InboundApi.Models;
using InboundApi.Services;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqFileProcessorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RabbitMqFileProcessorService> _logger;
    private readonly FileSettings _fileSettings;

    public RabbitMqFileProcessorService(
        IServiceScopeFactory scopeFactory, 
        ILogger<RabbitMqFileProcessorService> logger,
        FileSettings fileSettings
        )
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _fileSettings = fileSettings;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
            rabbitMqService.StartConsuming(OnMessageReceived); // Set up the message handler
            _logger.LogInformation(ResponseMessage.StartedConsumingMessages);
        }

        return Task.CompletedTask; // Keeps the service running without polling
    }

    private void OnMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var myLoggerService = scope.ServiceProvider.GetRequiredService<IMyLoggerService>();

            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var request = JsonSerializer.Deserialize<MyRequestModel>(message);

                if (request != null && request.FileContentLines.Count > 0)
                {
                    _ = ProcessMessageAsync(request, myLoggerService);
                }

                // Acknowledge the message
                var channel = ((EventingBasicConsumer)sender).Model;
                channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseMessage.ErrorProcessingMessage);
            }
        }
    }

    private async Task ProcessMessageAsync(MyRequestModel request, IMyLoggerService myLoggerService)
    {
        try
        {
            string filesFolderPath = _fileSettings.DestinationFolderPath;

            if (!Directory.Exists(filesFolderPath))
            {
                Directory.CreateDirectory(filesFolderPath);
            }

            string filePath = Path.Combine(filesFolderPath, request.FileName);

            await File.WriteAllLinesAsync(filePath, request.FileContentLines);

            await myLoggerService.LogAsync(request, "File Written");
            _logger.LogInformation($"{request.FileName} : {ResponseMessage.FileWrittenSuccessfully}");
        }
        catch (Exception ex)
        {
            await myLoggerService.LogAsync(request, "Error");
            _logger.LogError(ex, $"{ResponseMessage.ErrorProcessingFile} {request.FileName}");
        }
    }
}
