namespace Estapar.Domain.Dtos.Results;

/// <summary>
/// Represents a paginated result set, containing a subset of items along with pagination metadata.
/// </summary>
/// <remarks>This class is typically used to encapsulate the results of a paginated query, providing both the data
/// items and additional information such as the total number of items, the current page number, and the page
/// size.</remarks>
/// <typeparam name="T">The type of items contained in the result set.</typeparam>
/// <param name="items"></param>
/// <param name="totalCount"></param>
/// <param name="pageNumber"></param>
/// <param name="pageSize"></param>
public class PaginatedResult<T>(
    IEnumerable<T> items,
    int totalCount,
    int pageNumber,
    int pageSize
)
{
    /// <summary>
    /// Gets or sets the collection of items of type <typeparamref name="T"/>.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = items;

    /// <summary>
    /// Gets or sets the total count of items.
    /// </summary>
    public int TotalCount { get; set; } = totalCount;

    /// <summary>
    /// Gets or sets the current page number in a paginated collection.
    /// </summary>
    public int PageNumber { get; set; } = pageNumber;

    /// <summary>
    /// Gets or sets the number of items to display per page in a paginated list.
    /// </summary>
    public int PageSize { get; set; } = pageSize;

    /// <summary>
    /// Converts the current pagination data into a <see cref="PaginatedResult{T}"/> object containing the specified
    /// items.
    /// </summary>
    /// <typeparam name="Y">The type of the items in the paginated result.</typeparam>
    /// <param name="items">The list of items to include in the paginated result.</param>
    /// <returns>A <see cref="PaginatedResult{Y}"/> containing the provided items along with the current pagination metadata.</returns>
    public PaginatedResult<Y> ConvertPaginationData<Y>(
        IList<Y> items)
    {
        return new PaginatedResult<Y>(
            items, TotalCount, PageNumber, PageSize); ;
    }
}
