namespace GitIssuer.Application;

public record PaginationQuery(int? Page, int? Limit)
{
    public int Skip() => Page ?? 1;

    public int Take() => Limit ?? 10;
}