﻿namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(ShoppingCartItemDto ShoppingCartItem);
public record AddItemIntoBasketResponse(Guid Id);

internal class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/items", async ([FromRoute] string userName, [FromBody] AddItemIntoBasketRequest request, ISender sender) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);
            var result = await sender.Send(command);
            var response = result.Adapt<AddItemIntoBasketResponse>();

            return Results.Ok(response);
        }).Produces<AddItemIntoBasketResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Add Item Into Basket")
          .WithDescription("Add Item Into Basket");
    }
}
