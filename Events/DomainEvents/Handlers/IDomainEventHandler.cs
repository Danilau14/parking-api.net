namespace ParkingApi.Events.DomainEvents.Handlers;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}
