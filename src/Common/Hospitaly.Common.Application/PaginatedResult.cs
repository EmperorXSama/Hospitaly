using System.Text.Json.Serialization;

namespace Hospitaly.Common.Application;

public sealed record PaginatedResult<T>
{
    public List<T> Items { get; init; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / Math.Max(PageSize, 1));

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NextPage => Page < TotalPages ? Page + 1 : null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PreviousPage => Page > 1 ? Page - 1 : null;

    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
