namespace ParkingApi.Events;

public interface IDomainEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent);
}
