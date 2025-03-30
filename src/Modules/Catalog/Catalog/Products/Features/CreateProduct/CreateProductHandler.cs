
namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand
    (ProductDto Product)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductHandler(CatalogDbContext context)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = CreateNewProduct(command.Product);

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto product) =>
        Product.Create(product.Name, product.Category, product.Description, product.ImageFile, product.Price);
}
