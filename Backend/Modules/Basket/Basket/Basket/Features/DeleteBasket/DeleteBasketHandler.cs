﻿namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string Username)
    : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

internal class DeleteBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .SingleOrDefaultAsync(x => x.UserName == command.Username, cancellationToken);
       
        if (basket is null)
        {
            throw new BasketNotFoundException(command.Username);
        }

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteBasketResult(true);
    }
}
