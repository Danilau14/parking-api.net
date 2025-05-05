using ParkingApi.Application.Services.RabbitMQ;

namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RabbitMQController : ControllerBase
{
    private readonly RabbitMQService _rabbitMQService;

    public RabbitMQController(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishMessage([FromBody] MessageDto message)
    {
        await _rabbitMQService.PublishMessage(message);

        return Ok(new { Message = "Message sent to RabbitMQ" });
    }

    [HttpPost("consume")]
    public async Task<IActionResult> ConsumeMessage()
    {
        var message = await _rabbitMQService.ConsumeMessages();

        return Ok(new { Message = message });
    }
}
