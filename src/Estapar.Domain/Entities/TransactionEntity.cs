using Estapar.Domain.Entities.Base;

namespace Estapar.Domain.Entities;

/// <summary>
/// Represents a billing transaction for a vehicle stay in a parking facility.
/// </summary>
/// <remarks>
/// <para>A transaction is created when a vehicle completes a parking stay (entry + exit).
/// It calculates the total charge based on the stay duration and the park's price table:</para>
/// <list type="bullet">
/// <item><description>Stays within the grace period (e.g., 30 minutes) result in a zero charge</description></item>
/// <item><description>Stays exceeding the grace period are charged at the configured hourly rate</description></item>
/// </list>
/// <para><strong>Relationships:</strong></para>
/// <list type="bullet">
/// <item><description>One transaction references one entry traffic record (one-to-one)</description></item>
/// <item><description>One transaction references one exit traffic record (one-to-one)</description></item>
/// </list>
/// </remarks>
public class TransactionEntity : IEntityBase
{
    /// <summary>
    /// Gets the unique identifier for this transaction.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the entry traffic record associated with this transaction.
    /// </summary>
    public Guid EntryTrafficId { get; private set; }

    /// <summary>
    /// Gets the identifier of the exit traffic record associated with this transaction.
    /// </summary>
    public Guid ExitTrafficId { get; private set; }

    /// <summary>
    /// Gets the calculated charge for the vehicle stay.
    /// </summary>
    /// <value>
    /// Zero if the stay was within the grace period; otherwise, the hourly rate multiplied
    /// by the number of hours stayed (rounded up).
    /// </value>
    public decimal Balance { get; private set; }

    /// <summary>
    /// Gets the total duration of the vehicle stay in the parking facility.
    /// </summary>
    public TimeSpan StayDuration { get; private set; }

    /// <summary>
    /// Gets the timestamp when this transaction was created in the system.
    /// </summary>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this transaction.
    /// </summary>
    public DateTime? Updated { get; private set; }

    /// <summary>
    /// Gets the entry traffic record associated with this transaction.
    /// </summary>
    public virtual TrafficEntity EntryTraffic { get; private set; } = null!;

    /// <summary>
    /// Gets the exit traffic record associated with this transaction.
    /// </summary>
    public virtual TrafficEntity ExitTraffic { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Private constructor for ORM frameworks.
    /// </remarks>
    protected TransactionEntity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionEntity"/> class by calculating
    /// the charge based on the entry/exit traffic records and the park's price table.
    /// </summary>
    /// <param name="entryTrafficId">The identifier of the entry traffic record.</param>
    /// <param name="exitTrafficId">The identifier of the exit traffic record.</param>
    /// <param name="entryDate">The date and time the vehicle entered the parking facility.</param>
    /// <param name="exitDate">The date and time the vehicle exited the parking facility.</param>
    /// <param name="entryPrice">The dynamic hourly price that was locked in when the vehicle entered.</param>
    /// <param name="priceTable">The price table used to calculate the charge (grace period).</param>
    public TransactionEntity(
        Guid entryTrafficId,
        Guid exitTrafficId,
        DateTime entryDate,
        DateTime exitDate,
        decimal entryPrice,
        PriceTableEntity priceTable
        )
    {
        Id = Guid.NewGuid();
        EntryTrafficId = entryTrafficId;
        ExitTrafficId = exitTrafficId;
        StayDuration = exitDate - entryDate;
        Balance = priceTable.CalculateCharge(StayDuration, entryPrice);
        Created = DateTime.UtcNow;
    }
}
