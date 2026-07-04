using Microsoft.EntityFrameworkCore.Storage;

namespace Estapar.Domain.Contracts.Repositories.Base;

/// <summary>
/// Interface que define as operações de unidade de trabalho para um contexto específico do Entity Framework Core.
/// </summary>
/// <typeparam name="TContext"></typeparam>
public interface IUnitOfWork
{
    /// <summary>
    /// Commita a transação.
    /// </summary>
    /// <returns></returns>
    Task CommitAsync();

    /// <summary>
    /// Reverte a transação.
    /// </summary>
    /// <returns></returns>
    Task RollbackAsync();

    /// <summary>
    /// Começar transação.
    /// </summary>
    /// <returns></returns>
    Task<IDbContextTransaction> BeginTransactAsync();

    /// <summary>
    /// Finalizar transação.
    /// </summary>
    /// <returns></returns>
    Task CommitTransactAsync(IDbContextTransaction dbContextTransaction);

    /// <summary>
    /// Resetar transação.
    /// </summary>
    /// <returns></returns>
    Task RollBackTransactionAsync(IDbContextTransaction dbContextTransaction);
}
