﻿namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery()
    :IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<ProductDto> Products);

public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //get products using dbContext
        //return result

        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        //mapping product entity to productDto using Mapster
        var productsDtos = products.Adapt<List<ProductDto>>();
        
        return new GetProductsResult(productsDtos);
    }
}
