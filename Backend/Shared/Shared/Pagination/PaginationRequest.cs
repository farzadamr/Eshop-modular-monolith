namespace Shared.Pagination;

public record PaginationRequest(int pageSize = 10, int pageIndex = 0);
