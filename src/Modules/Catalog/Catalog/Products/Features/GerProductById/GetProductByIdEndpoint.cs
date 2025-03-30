﻿namespace Catalog.Products.Features.GerProductById;

//public record GetProductByIdRequest(Guid Id);
public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new GetProductByIdQuery(id);
            var result = await sender.Send(command);

            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        }).WithName("GetProductById")
          .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Get Product By Id")
          .WithDescription("Get Product By Id");
    }
}
