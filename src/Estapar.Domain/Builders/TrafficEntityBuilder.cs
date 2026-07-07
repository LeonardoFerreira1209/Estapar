using Estapar.Domain.Entities;
using Estapar.Domain.Enums.Traffic;

namespace Estapar.Domain.Builders;

/// <summary>
/// Provides a fluent interface for building and configuring a traffic entity.
/// </summary>
public sealed class TrafficEntityBuilder
{
    private string licensePlate = string.Empty;
    private DateTime date;
    private Guid laneId;
    private TrafficAction action;
    private bool success;
    private decimal balance;
    private TrafficError error = TrafficError.None;

    /// <summary>
    /// Sets the vehicle license plate for the traffic record being built.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <returns>The current <see cref="TrafficEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TrafficEntityBuilder AddLicensePlate(string licensePlate)
    {
        this.licensePlate = licensePlate;

        return this;
    }

    /// <summary>
    /// Sets the date and time of the traffic attempt.
    /// </summary>
    /// <param name="date">The date and time of the attempt.</param>
    /// <returns>The current <see cref="TrafficEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TrafficEntityBuilder AddDate(DateTime date)
    {
        this.date = date;

        return this;
    }

    /// <summary>
    /// Sets the lane identifier for the traffic record being built.
    /// </summary>
    /// <param name="laneId">The unique identifier of the lane where the attempt occurred.</param>
    /// <returns>The current <see cref="TrafficEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TrafficEntityBuilder AddLaneId(Guid laneId)
    {
        this.laneId = laneId;

        return this;
    }

    /// <summary>
    /// Sets the action type for the traffic record being built.
    /// </summary>
    /// <param name="action">The action type (entry or exit).</param>
    /// <returns>The current <see cref="TrafficEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TrafficEntityBuilder AddAction(TrafficAction action)
    {
        this.action = action;

        return this;
    }

    /// <summary>
    /// Sets whether the traffic attempt was successful.
    /// </summary>
    /// <param name="success">True if the attempt was successful; otherwise, false.</param>
    /// <returns>The current <see cref="TrafficEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TrafficEntityBuilder AddSuccess(bool success)
    {
        this.success = success;

        return this;
    }

    /// <summary>
    /// Sets the fixed balance amount for the traffic record being built.
    /// </summary>
    /// <param name="balance">The fixed entry/exit balance configured for the parking facility.</param>
    /// <returns>The current <see cref="TrafficEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TrafficEntityBuilder AddBalance(decimal balance)
    {
        this.balance = balance;

        return this;
    }

    /// <summary>
    /// Sets the error code for the traffic record being built.
    /// </summary>
    /// <param name="error">The error that occurred during the attempt.</param>
    /// <returns>The current <see cref="TrafficEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TrafficEntityBuilder AddError(TrafficError error)
    {
        this.error = error;

        return this;
    }

    /// <summary>
    /// Constructs and returns a new instance of <see cref="TrafficEntity"/> using the current state of the builder.
    /// </summary>
    /// <returns>A new instance of <see cref="TrafficEntity"/> populated with the builder's current state.</returns>
    public TrafficEntity Builder() =>
        new(
            licensePlate,
            date,
            laneId,
            action,
            success,
            balance,
            error
        );
}
