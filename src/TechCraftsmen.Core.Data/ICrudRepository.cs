// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

namespace TechCraftsmen.Core.Data;

public interface ICrudRepository<T> where T : Entity
{
    int Create(T entity);
    IQueryable<T> GetByFilter(DataFilter filter);
    T? GetById(int id);
    void Update(T entity);
    void Delete(int id);
}
