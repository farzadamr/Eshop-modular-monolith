 namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(string userName, ShoppingCartItemDto ShoppingCartItemDto)
    :ICommand<AddItemIntoBasketResult>;
public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(p => p.userName).NotEmpty().WithMessage("Username is required");
        RuleFor(p => p.ShoppingCartItemDto.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(p => p.ShoppingCartItemDto.Quantity).NotEmpty().WithMessage("Quantity is required");
    }
}

internal class AddItemIntoBasketHandler (BasketDbContext dbContext)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await dbContext.ShoppingCarts
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.UserName == command.userName, cancellationToken);

        if (shoppingCart is null)
        {
            throw new BasketNotFoundException(command.userName);
        }

        shoppingCart.AddItem(
            command.ShoppingCartItemDto.ProductId,
            command.ShoppingCartItemDto.Quantity,
            command.ShoppingCartItemDto.Color,
            command.ShoppingCartItemDto.Price,
            command.ShoppingCartItemDto.ProductName);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new AddItemIntoBasketResult(shoppingCart.Id);

    }
}
