using Estapar.Domain.Entities.Base;

namespace Estapar.Domain.Contracts.Repositories.Base;

/// <summary>
/// Defines a repository interface for performing bulk operations on entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository, which must be a class implementing <see
/// cref="IEntityPrimaryKey{Guid}"/>.</typeparam>
public interface IGenerictEntityCoreRepository<T> : IGenericRepository<T> where T : class, IEntityPrimaryKey<Guid>
{
   
}
