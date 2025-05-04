using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest)
    : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        query.PaginationRequest
            .Deconstruct(out var pageIndex, out var pageSize);

        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var productsDto = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(
            new PaginatedResult<ProductDto>(
                pageIndex,
                pageSize,
                totalCount,
                productsDto)
            );
    }
}
