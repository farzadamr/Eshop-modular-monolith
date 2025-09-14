using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest)
    :IQuery<GetProductsResult>;
public record GetProductsResult(PaginatedResult<ProductDto> Products);

public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //get products using dbContext
        //return result
        var pageIndex = query.PaginationRequest.pageIndex;
        var pageSize = query.PaginationRequest.pageSize;

        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);
        
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(page)
            .ToListAsync(cancellationToken);

        //mapping product entity to productDto using Mapster
        var productsDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(
                new PaginatedResult<ProductDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    productsDtos)
            );
    }
}
