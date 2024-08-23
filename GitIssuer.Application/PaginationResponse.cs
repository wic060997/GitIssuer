namespace GitIssuer.Application;

public record PaginationResponse<TModel>(
        IEnumerable<TModel> Data,
        PaginationModelResponse Pagination)
    ;

public record PaginationModelResponse(int? Limit = 0, int? Page = 0);