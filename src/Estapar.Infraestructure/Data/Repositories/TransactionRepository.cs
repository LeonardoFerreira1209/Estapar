using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Entities;
using Estapar.Infraestructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="TransactionEntity"/>.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class TransactionRepository(EstaparContext context)
    : GenericEntityCoreRepository<TransactionEntity>(context), ITransactionRepository
{
    /// <summary>
    /// Retrieves all billing transactions for the specified vehicle, including entry and exit traffic details.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate to search for.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="TransactionEntity"/>
    /// records with eagerly loaded entry and exit traffic data, ordered by creation date descending (most recent first).
    /// </returns>
    public async Task<IList<TransactionEntity>> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    ) => await context.Set<TransactionEntity>()
            .Include(t => t.EntryTraffic)
            .Include(t => t.ExitTraffic)
            .Where(t => t.EntryTraffic.LicensePlate == licensePlate)
            .OrderByDescending(t => t.Created)
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieves all billing transactions of a given park whose vehicle exit occurred on the specified date.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector) to calculate the billing for.</param>
    /// <param name="date">The date to filter the vehicle exits by.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="TransactionEntity"/>
    /// records with eagerly loaded entry and exit traffic data whose exit occurred on the given date, for the given park.
    /// </returns>
    public async Task<IList<TransactionEntity>> GetByParkIdAndDateAsync(
        Guid parkId,
        DateOnly date,
        CancellationToken cancellationToken = default
    )
    {
        var startDate = 
            DateTime.SpecifyKind(
                date.ToDateTime(TimeOnly.MinValue), 
                DateTimeKind.Utc
            );

        var endDate = startDate.AddDays(1);

        return await context.Set<TransactionEntity>()
            .Include(t => t.EntryTraffic)
            .Include(t => t.ExitTraffic)
                .ThenInclude(e => e.Lane)
            .Where(t => t.ExitTraffic.Lane.ParkId == parkId
                && t.ExitTraffic.Date >= startDate
                && t.ExitTraffic.Date < endDate)
            .OrderByDescending(t => t.Created)
            .ToListAsync(cancellationToken);
    }
}
