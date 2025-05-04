﻿
using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsRequest(PaginationRequest PaginationRequest);
public record GetProductsResponse(PaginatedResult<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var command = new GetProductsQuery(request);
            var result = await sender.Send(command);

            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        }).WithName("GetProducts")
          .Produces<GetProductsResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Get Products")
          .WithDescription("Get Products");
    }
}
