namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCart);
public record CreateBasketResponse(Guid Id);

internal class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateBasketResult>();

            return Results.Created($"/basket/{response.Id}", response);
        }).Produces<CreateBasketRequest>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Create basket")
          .WithDescription("Create basket");
    }
}
