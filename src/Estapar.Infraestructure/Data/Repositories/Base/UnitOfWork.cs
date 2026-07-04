using Estapar.Domain.Contracts.Repositories.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Estapar.Infraestructure.Data.Repositories.Base;

/// <summary>
/// Provides a unit of work pattern implementation for managing database transactions and context lifecycle.
/// </summary>
/// <remarks>The <see cref="UnitOfWork"/> class encapsulates a database context and provides methods to manage
/// transactions, commit changes, and handle connection lifecycle operations. It is designed to ensure that database
/// operations are performed in a consistent and controlled manner, supporting both transactional and non-transactional
/// workflows.  Typical usage involves creating an instance of <see cref="UnitOfWork"/>, performing operations on the
/// database context, and then calling <see cref="CommitAsync"/> to persist changes or <see cref="RollbackAsync"/> to
/// discard them. For transactional workflows, use <see cref="BeginTransactAsync"/> to start a transaction, followed by
/// <see cref="CommitTransactAsync"/> or <see cref="RollBackTransactionAsync"/> to finalize or revert the
/// transaction.</remarks>
/// <param name="context">The database context used for database operations.</param>
public class UnitOfWork(
    EstaparContext context
    ) : IUnitOfWork
{
    /// <summary>
    /// Commits all pending changes in the current context to the database asynchronously.
    /// </summary>
    /// <remarks>This method saves all changes made to the tracked entities in the context to the underlying
    /// database. It should be called after making modifications to entities to persist those changes.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task completes when all changes have been successfully
    /// saved.</returns>
    public async Task CommitAsync()
        => await context.SaveChangesAsync();

    /// <summary>
    /// Rolls back the current transaction asynchronously by disposing of the associated context.
    /// </summary>
    /// <remarks>This method releases resources held by the context and ensures that any pending changes are
    /// discarded. It should be called when a transaction needs to be aborted.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RollbackAsync()
        => await context.DisposeAsync();

    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <remarks>This method initiates a transaction on the underlying database connection. Use the returned
    /// <see cref="IDbContextTransaction"/> object to manage the transaction lifecycle,  including committing or rolling
    /// back the transaction.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
    /// cref="IDbContextTransaction"/> instance that can be used to control the transaction.</returns>
    public async Task<IDbContextTransaction> BeginTransactAsync()
        => await context.Database.BeginTransactionAsync();

    /// <summary>
    /// Commits the specified database transaction asynchronously.
    /// </summary>
    /// <param name="dbContextTransaction">The database transaction to commit. This parameter cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CommitTransactAsync(IDbContextTransaction dbContextTransaction)
        => await dbContextTransaction.CommitAsync();

    /// <summary>
    /// Rolls back the specified database transaction asynchronously.
    /// </summary>
    /// <remarks>Use this method to revert changes made during the transaction. Ensure that the <paramref
    /// name="dbContextTransaction"/> is not null and represents an active transaction before calling this
    /// method.</remarks>
    /// <param name="dbContextTransaction">The <see cref="IDbContextTransaction"/> instance representing the transaction to roll back.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RollBackTransactionAsync(IDbContextTransaction dbContextTransaction)
        => await dbContextTransaction.RollbackAsync();
}
