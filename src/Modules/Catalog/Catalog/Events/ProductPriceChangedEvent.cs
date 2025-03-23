namespace Catalog.Events;

public record ProductPriceChangedEvent(Product Product)
    : IDomainEvent;