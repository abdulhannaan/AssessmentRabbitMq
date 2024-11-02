using InboundApi.Data;
using InboundApi.Helpers;
using InboundApi.Models;
using InboundApi.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqFileProcessorService : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RabbitMqFileProcessorService> _logger;
    private readonly FileSettings _fileSettings;

    public RabbitMqFileProcessorService(
        IRabbitMqService rabbitMqService,
        IServiceScopeFactory scopeFactory,
        ILogger<RabbitMqFileProcessorService> logger,
        IOptions<FileSettings> fileSettings)
    {
        _rabbitMqService = rabbitMqService;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _fileSettings = fileSettings.Value;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMqService.StartConsuming(OnRequestReceived);
        _logger.LogInformation(ResponseMessage.StartedConsumingMessages);

        return Task.CompletedTask;
    }

    private void OnRequestReceived(object sender, BasicDeliverEventArgs eventArgs)
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
                    myLoggerService.LogAsync(request, StatusEnum.SuccessfullyConsumed.ToString());
                    _ = WritingFileToDestination(request);
                    myLoggerService.LogAsync(request, StatusEnum.FileWritten.ToString());

                }

                var channel = ((EventingBasicConsumer)sender).Model;
                channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                 myLoggerService.LogAsync(new MyRequestModel(), ResponseMessage.Error);
                _logger.LogError(ex, ResponseMessage.ErrorConsumingRequest);
            }
        }
    }

    private async Task WritingFileToDestination(MyRequestModel request)
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

            _logger.LogInformation($"{request.FileName} : {ResponseMessage.FileWrittenSuccessfully}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{ResponseMessage.ErrorProcessingFile} {request.FileName}");
        }
    }
}
