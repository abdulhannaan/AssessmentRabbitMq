using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InboundApi.Models;
using InboundApi.Services;
using System.Net;
using InboundApi.Data;
using InboundApi.Helpers;

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
        await _myLoggerService.LogAsync(request, StatusEnum.Received.ToString());

        try
        {
            await _rabbitMqService.SendMessageToQueueAsync(request);

            await _myLoggerService.LogAsync(request, StatusEnum.SuccessfullyProcessed.ToString());

            var response = new MyResponseModel
            {
                ReturnCode = HttpStatusCode.OK,
                FileName = request.FileName,
                Status = StatusEnum.SuccessfullyProcessed.ToString()
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            await _myLoggerService.LogAsync(request, StatusEnum.ErrorProcessing.ToString());

            return StatusCode(500, new
            {
                ReturnCode = HttpStatusCode.InternalServerError,
                Message = ResponseMessage.ErrorProcessingRequest,
                Details = ex.Message
            });
        }
    }
}
