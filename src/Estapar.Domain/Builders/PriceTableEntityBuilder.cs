using Estapar.Domain.Entities;

namespace Estapar.Domain.Builders;

/// <summary>
/// Provides a fluent interface for building and configuring a price table entity.
/// </summary>
public sealed class PriceTableEntityBuilder
{
    private Guid parkId;
    private decimal hourlyRate;
    private int gracePeriodMinutes = 30;

    /// <summary>
    /// Sets the park identifier for the price table being built.
    /// </summary>
    /// <param name="parkId">The unique identifier of the parent park.</param>
    /// <returns>The current <see cref="PriceTableEntityBuilder"/> instance, allowing for method chaining.</returns>
    public PriceTableEntityBuilder AddParkId(Guid parkId)
    {
        this.parkId = parkId;

        return this;
    }

    /// <summary>
    /// Sets the hourly rate for the price table being built.
    /// </summary>
    /// <param name="hourlyRate">The monetary value charged per hour after the grace period.</param>
    /// <returns>The current <see cref="PriceTableEntityBuilder"/> instance, allowing for method chaining.</returns>
    public PriceTableEntityBuilder AddHourlyRate(decimal hourlyRate)
    {
        this.hourlyRate = hourlyRate;

        return this;
    }

    /// <summary>
    /// Sets the grace period in minutes for the price table being built.
    /// </summary>
    /// <param name="gracePeriodMinutes">The number of minutes before billing starts. Defaults to 30.</param>
    /// <returns>The current <see cref="PriceTableEntityBuilder"/> instance, allowing for method chaining.</returns>
    public PriceTableEntityBuilder AddGracePeriodMinutes(int gracePeriodMinutes)
    {
        this.gracePeriodMinutes = gracePeriodMinutes;

        return this;
    }

    /// <summary>
    /// Constructs and returns a new instance of <see cref="PriceTableEntity"/> using the current state of the builder.
    /// </summary>
    /// <returns>A new instance of <see cref="PriceTableEntity"/> populated with the builder's current state.</returns>
    public PriceTableEntity Builder() =>
        new(
            parkId,
            hourlyRate,
            gracePeriodMinutes
        );
}
