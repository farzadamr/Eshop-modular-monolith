namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid productId)
    :ICommand<DeleteProductResult>;
public record DeleteProductResult(bool isSuccess);
public class DeleteProductCommandValidator: AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.productId).NotEmpty().WithMessage("ProductId is required");
    }
}
public class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync([command.productId], cancellationToken: cancellationToken);
        
        if (product is null)
            throw new ProductNotFoundException(command.productId);
        
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
