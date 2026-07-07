namespace Estapar.Domain.Dtos.Response;

/// <summary>
/// Response DTO for the billing (revenue) totals of a park on a specific date.
/// </summary>
public class ParkRevenueResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the park.
    /// </summary>
    public Guid ParkId { get; set; }

    /// <summary>
    /// Gets or sets the date this revenue calculation refers to.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the total billed amount for the park on the given date.
    /// </summary>
    /// <value>
    /// The sum of <c>Balance</c> from every billing transaction whose vehicle exit occurred on the requested date.
    /// </value>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the currency of the billed amount.
    /// </summary>
    public string Currency { get; set; } = "BRL";

    /// <summary>
    /// Gets or sets the number of billing transactions that compose the total amount.
    /// </summary>
    public int TransactionsCount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp (UTC) when this revenue was calculated.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
