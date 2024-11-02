using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InboundApi.Models;
using InboundApi.Services;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class InboundController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IMyLoggerService _myLoggerService;

    public InboundController(IRabbitMqService rabbitMqService, IMyLoggerService myLoggerService)
    {
        _rabbitMqService = rabbitMqService;
        _myLoggerService = myLoggerService;
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveRequest([FromBody] MyRequestModel request)
    {
        await _myLoggerService.LogAsync(request, "Received");

        try
        {
            // Send the message to RabbitMQ
            await _rabbitMqService.SendMessageToQueueAsync(request);

            // Log success after sending
            await _myLoggerService.LogAsync(request, "Successfully Processed");

            var response = new MyResponseModel
            {
                ReturnCode = HttpStatusCode.OK,
                FileName = request.FileName,
                Status = "Successfully Processed"
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            // Log failure if there is an exception
            await _myLoggerService.LogAsync(request, $"Failed: {ex.Message}");

            return StatusCode(500, new
            {
                ReturnCode = HttpStatusCode.InternalServerError,
                Message = "An error occurred while processing the request.",
                Details = ex.Message
            });
        }
    }
}
