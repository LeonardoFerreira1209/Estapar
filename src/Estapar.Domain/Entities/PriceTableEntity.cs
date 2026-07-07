using Estapar.Domain.Entities.Base;

namespace Estapar.Domain.Entities;

/// <summary>
/// Represents the price table configuration for a parking facility.
/// </summary>
/// <remarks>
/// <para>Each park has a single price table that defines how vehicle stays are billed:</para>
/// <list type="bullet">
/// <item><description><strong>Grace Period:</strong> Stays up to the configured grace period (e.g., 30 minutes) are not charged</description></item>
/// <item><description><strong>Hourly Rate:</strong> Stays exceeding the grace period are charged based on the configured hourly rate</description></item>
/// </list>
/// <para><strong>Relationships:</strong></para>
/// <list type="bullet">
/// <item><description>One price table belongs to one park (one-to-one)</description></item>
/// </list>
/// </remarks>
public class PriceTableEntity : IEntityBase
{
    /// <summary>
    /// Gets the unique identifier for the price table.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the park to which this price table belongs.
    /// </summary>
    public Guid ParkId { get; private set; }

    /// <summary>
    /// Gets the hourly rate charged for vehicle stays that exceed the grace period.
    /// </summary>
    /// <value>
    /// The monetary value charged per hour after the grace period has elapsed.
    /// </value>
    public decimal HourlyRate { get; private set; }

    /// <summary>
    /// Gets the grace period in minutes during which no charge is applied.
    /// </summary>
    /// <value>
    /// The number of minutes a vehicle may stay without incurring a charge. Defaults to 30 minutes.
    /// </value>
    public int GracePeriodMinutes { get; private set; }

    /// <summary>
    /// Gets the timestamp when this price table was initially created in the system.
    /// </summary>
    public DateTime Created { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent modification to this price table.
    /// </summary>
    public DateTime? Updated { get; private set; }

    /// <summary>
    /// Gets the park to which this price table belongs.
    /// </summary>
    public virtual ParkEntity Park { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriceTableEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Private constructor for ORM frameworks.
    /// </remarks>
    protected PriceTableEntity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PriceTableEntity"/> class with the specified configuration.
    /// </summary>
    /// <param name="parkId">The identifier of the park to which this price table belongs.</param>
    /// <param name="hourlyRate">The hourly rate charged after the grace period.</param>
    /// <param name="gracePeriodMinutes">The grace period in minutes before charges apply. Defaults to 30.</param>
    public PriceTableEntity(
        Guid parkId,
        decimal hourlyRate,
        int gracePeriodMinutes = 30
        )
    {
        Id = Guid.NewGuid();
        ParkId = parkId;
        HourlyRate = hourlyRate;
        GracePeriodMinutes = gracePeriodMinutes;
        Created = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the price table configuration.
    /// </summary>
    /// <param name="hourlyRate">The new hourly rate.</param>
    /// <param name="gracePeriodMinutes">The new grace period in minutes.</param>
    public void Update(decimal hourlyRate, int gracePeriodMinutes)
    {
        HourlyRate = hourlyRate;
        GracePeriodMinutes = gracePeriodMinutes;
        Updated = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the charge for a vehicle stay based on the given duration, using this price table's configured <see cref="HourlyRate"/>.
    /// </summary>
    /// <param name="stayDuration">The total duration of the vehicle stay.</param>
    /// <returns>
    /// The calculated charge. Returns zero if the stay is within the grace period;
    /// otherwise, returns the hourly rate multiplied by the number of hours (rounded up).
    /// </returns>
    public decimal CalculateCharge(TimeSpan stayDuration)
        => CalculateCharge(stayDuration, HourlyRate);

    /// <summary>
    /// Calculates the charge for a vehicle stay based on the given duration and a fixed hourly rate.
    /// </summary>
    /// <param name="stayDuration">The total duration of the vehicle stay.</param>
    /// <param name="hourlyRate">
    /// The hourly rate to apply after the grace period. Used to charge the dynamic price that was
    /// locked in at the time the vehicle entered, instead of the price table's current <see cref="HourlyRate"/>.
    /// </param>
    /// <returns>
    /// The calculated charge. Returns zero if the stay is within the grace period;
    /// otherwise, returns the given hourly rate multiplied by the number of hours (rounded up).
    /// </returns>
    public decimal CalculateCharge(
        TimeSpan stayDuration, 
        decimal hourlyRate
        )
    {
        if (stayDuration.TotalMinutes <= GracePeriodMinutes)
            return 0m;

        var chargeableHours = 
            Math.Ceiling(
                stayDuration.TotalHours
            );

        return hourlyRate * (decimal)chargeableHours;
    }

    /// <summary>
    /// Calculates the dynamic entry price based on the current occupancy of the park (sector),
    /// applying a discount or surcharge relative to the base <see cref="HourlyRate"/>.
    /// </summary>
    /// <param name="occupiedSpots">The number of spots (garages) currently occupied in the park.</param>
    /// <param name="totalCapacity">The total number of spots (garages) available in the park.</param>
    /// <returns>
    /// The adjusted price: a 10% discount when occupancy is below 25%, the base price up to 50%,
    /// a 10% surcharge up to 75%, and a 25% surcharge above 75% occupancy. Treated as fully occupied
    /// (maximum surcharge) when <paramref name="totalCapacity"/> is zero or less.
    /// </returns>
    public decimal CalculateEntryPrice(
        int occupiedSpots, 
        int totalCapacity
        )
    {
        var occupancyRate = totalCapacity <= 0
            ? 1m
            : (decimal)occupiedSpots / totalCapacity;

        var multiplier = occupancyRate switch
        {
            < 0.25m => 0.90m,
            <= 0.50m => 1.00m,
            <= 0.75m => 1.10m,
            _ => 1.25m
        };

        return HourlyRate * multiplier;
    }
}
