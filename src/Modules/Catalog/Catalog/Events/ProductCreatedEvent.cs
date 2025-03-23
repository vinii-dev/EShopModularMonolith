namespace Catalog.Events;

public record ProductCreatedEvent(Product Product)
    : IDomainEvent;

