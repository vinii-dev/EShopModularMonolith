
namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;
public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greather than 0");
    }
}

internal class AddItemIntoBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (shoppingCart == null)
            throw new BasketNotFoundException(command.UserName);

        var shoppingCartItem = command.ShoppingCartItem;
        shoppingCart.AddItem(productId: shoppingCartItem.ProductId,
                             quantity: shoppingCartItem.Quantity,
                             color: shoppingCartItem.Color,
                             price: shoppingCartItem.Price,
                             productName: shoppingCartItem.ProductName);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddItemIntoBasketResult(shoppingCartItem.Id);
    }
}
