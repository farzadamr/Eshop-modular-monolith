﻿
namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category)
    : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);

public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        
         var productDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsByCategoryResult(productDtos);
    }
}
