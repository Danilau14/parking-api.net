namespace ParkingApi.Events.DomainEvents;

public class VehicleRegisteredEvent : IDomainEvent
{
    public string Email { get; }
    public string LicensePlate { get; }
    public int ParkingLotId { get; }

    public VehicleRegisteredEvent(string email, string licensePlate, int parkingLotId)
    {
        Email = email;
        LicensePlate = licensePlate;
        ParkingLotId = parkingLotId;
    }
}
