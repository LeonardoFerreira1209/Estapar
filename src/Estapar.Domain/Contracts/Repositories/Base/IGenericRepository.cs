using Estapar.Domain.Dtos.Results;
using Estapar.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Estapar.Domain.Contracts.Repositories.Base;

/// <summary>
/// Defines a generic repository interface for performing CRUD operations on entities with a primary key of type <see
/// cref="Guid"/>.
/// </summary>
/// <remarks>This interface provides asynchronous methods for creating, updating, retrieving, and deleting
/// entities. It supports operations with optional filtering and pagination.</remarks>
/// <typeparam name="T">The type of the entity, which must implement <see cref="IEntityPrimaryKey{Guid}"/>.</typeparam>
public interface IGenericRepository<T> where T : IEntityPrimaryKey<Guid>
{
    /// <summary>
    /// Asynchronously creates a new entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to be created. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created entity.</returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Asynchronously updates the specified entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to update. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result contains the updated entity.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity of type <typeparamref
    /// name="T"/> if found; otherwise, <see langword="null"/>.</returns>
    Task<T> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves an entity that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each entity for a condition. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity that matches the
    /// predicate, or <see langword="null"/> if no such entity is found.</returns>
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves an entity that matches the specified predicate with optional includes.
    /// </summary>
    /// <param name="predicate">A function to test each entity for a condition. Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">A function to configure includes for related entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity that matches the
    /// predicate, or <see langword="null"/> if no such entity is found.</returns>
    Task<T> GetAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken,
        Func<IQueryable<T>, IQueryable<T>> includes = null);

    /// <summary>
    /// Asynchronously retrieves all entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">An optional expression to filter the entities. If null, all entities are retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that match the
    /// predicate.</returns>
    public Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);

    /// <summary>
    /// Retrieves a paginated list of items that match the specified criteria.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of items per page. Must be greater than 0.</param>
    /// <param name="predicate">An optional expression to filter the items. If null, all items are considered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedResponse{T}"/>
    /// with the items for the specified page and the total count of items.</returns>
    Task<PaginatedResult<T>> GetAllAsyncPaginated(
         int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null);

    /// <summary>
    /// Asynchronously deletes the specified entity.
    /// </summary>
    /// <param name="entidade">The entity to be deleted. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(T entidade);
}
