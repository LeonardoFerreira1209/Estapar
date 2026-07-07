using Estapar.Domain.Contracts.Repositories.Base;
using Estapar.Domain.Dtos.Results;
using Estapar.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Estapar.Infraestructure.Data.Repositories.Base;

/// <summary>
/// Provides a generic repository implementation for managing entities in an Entity Framework Core context.
/// </summary>
/// <remarks>This repository offers common data access operations such as creating, updating, deleting, and
/// retrieving entities. It supports both single-entity operations and batch operations, as well as paginated
/// queries.</remarks>
/// <typeparam name="T">The type of the entity managed by the repository. Must be a class that implements <see
/// cref="IEntityPrimaryKey{Guid}"/>.</typeparam>
/// <param name="context"></param>
public class GenericEntityCoreRepository<T>(EstaparContext context)
    : IGenerictEntityCoreRepository<T> where T : class, IEntityPrimaryKey<Guid>
{
    /// <summary>
    /// Asynchronously adds the specified entity to the database context.
    /// </summary>
    /// <param name="entity">The entity to add to the database. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity that was added.</returns>
    public virtual async Task<T> CreateAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);

        return entity;
    }

    /// <summary>
    /// Updates the specified entity in the data context and returns the updated entity.
    /// </summary>
    /// <param name="entity">The entity to be updated. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity.</returns>
    public virtual async Task<T> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);

        return await
            Task.FromResult(entity);
    }

    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <remarks>This method queries the underlying data source for an entity with the specified identifier.
    /// If no matching entity is found, the result will be <see langword="null"/>.</remarks>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity  of type <typeparamref
    /// name="T"/> if found; otherwise, <see langword="null"/>.</returns>
    public virtual Task<T> GetByIdAsync(Guid id)
        => context.Set<T>().FirstOrDefaultAsync(entity => entity.Id.Equals(id));

    /// <summary>
    /// Asynchronously retrieves the first entity that matches the specified predicate.
    /// </summary>
    /// <remarks>This method queries the underlying data source using the provided predicate. If no predicate is
    /// specified, it retrieves the first entity in the set. Ensure that the predicate is valid and compatible with the
    /// data source's query provider.</remarks>
    /// <param name="predicate">An optional expression used to filter the entities. If <paramref name="predicate"/> is null, the method retrieves
    /// the first entity in the set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the
    /// predicate, or <see langword="null"/> if no matching entity is found.</returns>
    public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        => await context.Set<T>().FirstOrDefaultAsync(predicate);

    /// <summary>
    /// Asynchronously retrieves the first entity that matches the specified predicate with optional includes.
    /// </summary>
    /// <param name="predicate">An expression used to filter the entities.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="includes">A function to configure includes for related entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the
    /// predicate with includes applied, or <see langword="null"/> if no matching entity is found.</returns>
    public virtual async Task<T> GetAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken,
        Func<IQueryable<T>, IQueryable<T>> includes = null)
    {
        IQueryable<T> query = context.Set<T>();

        if (includes != null)
        {
            query = includes(query);
            query = query.AsQueryable();
        }

        if (predicate != null)
            query = query.Where(predicate);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously counts the number of entities in the data set that satisfy the specified condition.
    /// </summary>
    /// <remarks>This method uses the underlying database provider to perform the count operation, which may
    /// involve executing a SQL query. For large data sets, the performance of this operation depends on the efficiency
    /// of the database provider.</remarks>
    /// <param name="predicate">An optional expression used to filter the entities to be counted. If <see langword="null"/>, all entities in the
    /// data set are counted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities that match
    /// the specified condition.</returns>
    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
    {
        IQueryable<T> query = context.Set<T>();

        if (predicate != null) query = query.Where(predicate);

        return await query.CountAsync();
    }

    /// <summary>
    /// Asynchronously retrieves all entities of type <typeparamref name="T"/> that match the specified predicate.
    /// </summary>
    /// <remarks>This method queries the underlying data source using the provided predicate, if specified, and
    /// returns the results as a list.  If no predicate is provided, all entities of type <typeparamref name="T"/> are
    /// returned.</remarks>
    /// <param name="predicate">An optional expression used to filter the entities. If <paramref name="predicate"/> is <see langword="null"/>,
    /// all entities are retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities of type
    /// <typeparamref name="T"/> that match the specified predicate.</returns>
    public virtual async Task<IList<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate = null)
    {
        IQueryable<T> query = context.Set<T>().AsNoTracking();

        if (predicate != null)
            query = MountDynamicWhere(
                query, predicate);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Asynchronously deletes the specified entity from the data context.
    /// </summary>
    /// <param name="entidade">The entity to be deleted. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public virtual async Task DeleteAsync(T entidade)
    {
        context.Remove(entidade);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves a paginated list of entities that match the specified criteria.
    /// </summary>
    /// <remarks>The entities are ordered by their identifier before pagination is applied. If the <paramref
    /// name="predicate"/> parameter  is provided, only entities matching the filter will be included in the
    /// result.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of items to include in each page. Must be greater than or equal to 1.</param>
    /// <param name="predicate">An optional filter expression to apply to the query. If null, all entities are included.</param>
    /// <returns>A <see cref="PaginatedResult{T}"/> containing the items for the specified page, the total count of matching
    /// entities,  and pagination details.</returns>
    public virtual async Task<PaginatedResult<T>> GetAllAsyncPaginated(
         int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null)
    {
        IQueryable<T> query = context.Set<T>().AsNoTracking();

        if (predicate != null)
            query = MountDynamicWhere(
                query, predicate);

        int skip = (pageNumber - 1) * pageSize;
        int totalCount = await query.CountAsync();

        var items
            = await query
                .OrderBy(x => x.Id)
                    .Skip(skip)
                        .Take(pageSize).ToListAsync();

        return new PaginatedResult<T>(
            items, totalCount, pageNumber, pageSize
        );
    }

    /// <summary>
    /// Applies a dynamic filtering condition to the specified query.
    /// </summary>
    /// <param name="query">The <see cref="IQueryable{T}"/> to which the filtering condition will be applied.</param>
    /// <param name="predicate">An expression that defines the filtering condition to apply to the query.</param>
    /// <returns>A new <see cref="IQueryable{T}"/> that includes the elements from the original query that satisfy the specified
    /// filtering condition.</returns>
    private static IQueryable<T> MountDynamicWhere(
        IQueryable<T> query,
            Expression<Func<T, bool>> predicate) => query.Where(predicate);
}
