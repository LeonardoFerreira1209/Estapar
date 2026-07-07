using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Entities;

namespace Estapar.Domain.Contracts.Repositories;

/// <summary>
/// Defines the repository contract for performing data access operations on <see cref="TransactionEntity"/>.
/// </summary>
public interface ITransactionRepository : IGenerictEntityCoreRepository<TransactionEntity>
{
    /// <summary>
    /// Asynchronously retrieves all transactions for a given vehicle license plate,
    /// including the associated entry and exit traffic records.
    /// </summary>
    /// <param name="licensePlate">The vehicle license plate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="TransactionEntity"/>
    /// records with related traffic data, ordered by creation date descending.
    /// </returns>
    Task<IList<TransactionEntity>> GetByLicensePlateAsync(
        string licensePlate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Asynchronously retrieves all billing transactions of a given park whose vehicle exit occurred
    /// on the specified date, including the associated entry and exit traffic records.
    /// </summary>
    /// <param name="parkId">The unique identifier of the park (sector) to calculate the billing for.</param>
    /// <param name="date">The date to filter the vehicle exits by.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a list of <see cref="TransactionEntity"/>
    /// records with related traffic data whose exit occurred on the given date, for the given park.
    /// </returns>
    Task<IList<TransactionEntity>> GetByParkIdAndDateAsync(
        Guid parkId,
        DateOnly date,
        CancellationToken cancellationToken = default
    );
}
