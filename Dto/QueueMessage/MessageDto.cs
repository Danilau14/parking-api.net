using ParkingApi.Core.Enums;

namespace ParkingApi.Dto.QueueMessage;

public class MessageDto
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public required string Entity { get; set; }
    public required Actions Action { get; set; }
    public required bool State {  get; set; }

    public required int? UserId { get; set; }
    public string Response { get; set; } = "new resourse";
}
