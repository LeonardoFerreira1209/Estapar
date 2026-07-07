using Estapar.Domain.Entities;

namespace Estapar.Domain.Builders;

/// <summary>
/// Provides a fluent interface for building and configuring a transaction entity.
/// </summary>
public sealed class TransactionEntityBuilder
{
    private Guid entryTrafficId;
    private Guid exitTrafficId;
    private DateTime entryDate;
    private DateTime exitDate;
    private decimal entryPrice;
    private PriceTableEntity priceTable = null!;

    /// <summary>
    /// Sets the entry traffic identifier for the transaction being built.
    /// </summary>
    /// <param name="entryTrafficId">The unique identifier of the entry traffic record.</param>
    /// <returns>The current <see cref="TransactionEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TransactionEntityBuilder AddEntryTrafficId(Guid entryTrafficId)
    {
        this.entryTrafficId = entryTrafficId;

        return this;
    }

    /// <summary>
    /// Sets the exit traffic identifier for the transaction being built.
    /// </summary>
    /// <param name="exitTrafficId">The unique identifier of the exit traffic record.</param>
    /// <returns>The current <see cref="TransactionEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TransactionEntityBuilder AddExitTrafficId(Guid exitTrafficId)
    {
        this.exitTrafficId = exitTrafficId;

        return this;
    }

    /// <summary>
    /// Sets the entry date and time for the transaction being built.
    /// </summary>
    /// <param name="entryDate">The date and time the vehicle entered the parking facility.</param>
    /// <returns>The current <see cref="TransactionEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TransactionEntityBuilder AddEntryDate(DateTime entryDate)
    {
        this.entryDate = entryDate;

        return this;
    }

    /// <summary>
    /// Sets the exit date and time for the transaction being built.
    /// </summary>
    /// <param name="exitDate">The date and time the vehicle exited the parking facility.</param>
    /// <returns>The current <see cref="TransactionEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TransactionEntityBuilder AddExitDate(DateTime exitDate)
    {
        this.exitDate = exitDate;

        return this;
    }

    /// <summary>
    /// Sets the dynamic hourly price that was locked in when the vehicle entered.
    /// </summary>
    /// <param name="entryPrice">The dynamic hourly price calculated at entry time based on occupancy.</param>
    /// <returns>The current <see cref="TransactionEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TransactionEntityBuilder AddEntryPrice(decimal entryPrice)
    {
        this.entryPrice = entryPrice;

        return this;
    }

    /// <summary>
    /// Sets the price table used to calculate the charge for the transaction being built.
    /// </summary>
    /// <param name="priceTable">The price table of the parking facility.</param>
    /// <returns>The current <see cref="TransactionEntityBuilder"/> instance, allowing for method chaining.</returns>
    public TransactionEntityBuilder AddPriceTable(PriceTableEntity priceTable)
    {
        this.priceTable = priceTable;

        return this;
    }

    /// <summary>
    /// Constructs and returns a new instance of <see cref="TransactionEntity"/> using the current state of the builder.
    /// </summary>
    /// <returns>A new instance of <see cref="TransactionEntity"/> populated with the builder's current state.</returns>
    public TransactionEntity Builder() =>
        new(
            entryTrafficId,
            exitTrafficId,
            entryDate,
            exitDate,
            entryPrice,
            priceTable
        );
}
