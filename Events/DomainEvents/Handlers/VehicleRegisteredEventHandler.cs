namespace ParkingApi.Events.DomainEvents.Handlers;

public class VehicleRegisteredEventHandler : IDomainEventHandler<VehicleRegisteredEvent>
{
    private readonly IRabbitMQSendMail _rabbitMQSendMail;

    public VehicleRegisteredEventHandler(IRabbitMQSendMail rabbitMQSendMail)
    {
        _rabbitMQSendMail = rabbitMQSendMail;
    }

    public async Task HandleAsync(VehicleRegisteredEvent ev)
    {
        try
        {
            await _rabbitMQSendMail.PublishAuditMessageAsync(
                ev.Email,
                "Vehicle in Parking lot",
                $"Vehicle with LicensePlate {ev.LicensePlate} in ParkingLot {ev.ParkingLotId}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
