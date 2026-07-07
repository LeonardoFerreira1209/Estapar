using Estapar.Domain.Builders;
using Estapar.Domain.Dtos.Response;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Extensions;

/// <summary>
/// Extension methods for creating TransactionEntity instances from traffic records.
/// </summary>
public static class TransactionExtensions
{
    /// <summary>
    /// Converts an entry/exit traffic pair into a <see cref="TransactionEntity"/>, calculating the charge from the
    /// dynamic price locked in at entry time (<see cref="TrafficEntity.Balance"/>) and the price table's grace period.
    /// </summary>
    /// <param name="entryTraffic">The traffic record generated when the vehicle entered, whose <see cref="TrafficEntity.Balance"/> holds the locked-in entry price.</param>
    /// <param name="exitTraffic">The traffic record generated when the vehicle exited.</param>
    /// <param name="priceTable">The price table used to calculate the charge for the stay.</param>
    /// <returns>A new <see cref="TransactionEntity"/> instance.</returns>
    public static TransactionEntity ToTransactionEntity(
        this TrafficEntity entryTraffic,
        TrafficEntity exitTraffic,
        PriceTableEntity priceTable
        )
        => new TransactionEntityBuilder()
            .AddEntryTrafficId(entryTraffic.Id)
            .AddExitTrafficId(exitTraffic.Id)
            .AddEntryDate(entryTraffic.Date)
            .AddExitDate(exitTraffic.Date)
            .AddEntryPrice(entryTraffic.Balance)
            .AddPriceTable(priceTable)
            .Builder();

    /// <summary>
    /// Converts a list of billing transactions into a <see cref="ParkRevenueResponse"/>, summing the
    /// <see cref="TransactionEntity.Balance"/> of every transaction to compute the total billed amount.
    /// </summary>
    /// <param name="transactions">The transactions billed for the park on the given date.</param>
    /// <param name="parkId">The unique identifier of the park the revenue refers to.</param>
    /// <param name="date">The date the revenue refers to.</param>
    /// <returns>A new <see cref="ParkRevenueResponse"/> instance with the aggregated billing totals.</returns>
    public static ParkRevenueResponse ToRevenueResponse(
        this IList<TransactionEntity> transactions,
        Guid parkId,
        DateOnly date
        )
        => new()
        {
            ParkId = parkId,
            Date = date,
            Amount = transactions.Sum(t => t.Balance),
            Currency = "BRL",
            TransactionsCount = transactions.Count,
            Timestamp = DateTime.UtcNow
        };
}
