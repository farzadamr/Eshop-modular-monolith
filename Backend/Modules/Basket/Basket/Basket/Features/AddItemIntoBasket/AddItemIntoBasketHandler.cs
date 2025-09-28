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

internal class AddItemIntoBasketHandler 
    (IBasketRepository repository, ISender sender)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetBasket(command.userName, false, cancellationToken);

        var result = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItemDto.ProductId));

        shoppingCart.AddItem(
            command.ShoppingCartItemDto.ProductId,
            command.ShoppingCartItemDto.Quantity,
            command.ShoppingCartItemDto.Color,
            result.Product.Price,
            result.Product.Name);

        await repository.SaveChangesAsync(command.userName, cancellationToken);
        
        return new AddItemIntoBasketResult(shoppingCart.Id);

    }
}
