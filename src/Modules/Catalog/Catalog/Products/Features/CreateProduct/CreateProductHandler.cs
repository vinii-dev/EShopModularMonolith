namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product)
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Product.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(c => c.Product.Category)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleFor(c => c.Product.ImageFile)
            .NotEmpty()
            .WithMessage("ImageFile is required");

        RuleFor(c => c.Product.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}

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

    private static Product CreateNewProduct(ProductDto product) =>
        Product.Create(product.Name, product.Category, product.Description, product.ImageFile, product.Price);
}
