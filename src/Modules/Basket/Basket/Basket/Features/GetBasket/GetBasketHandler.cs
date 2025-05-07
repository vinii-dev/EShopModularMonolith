﻿namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName)
    : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

internal class GetBasketHandler(BasketDbContext dbContext)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.UserName == query.UserName, cancellationToken);

        if (basket == null)
            throw new BasketNotFoundException(query.UserName);

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}
