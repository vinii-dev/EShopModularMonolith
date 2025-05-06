using Shared.Exceptions;

namespace Basket.Basket.Exceptions;

internal class BasketNotFoundException(string userName)
    : NotFoundException("ShoppingCart", userName)
{
}
