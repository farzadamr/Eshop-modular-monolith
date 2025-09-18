namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string userName)
    : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

internal class GetBasketHandler (IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.userName, true, cancellationToken);
        var basketDto = basket.Adapt<ShoppingCartDto>();
        
        return new GetBasketResult(basketDto);
    }
}
