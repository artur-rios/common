namespace TechCraftsmen.Core.Data;

public interface ICrudRepository<T> where T : Entity
{
    int Create(T entity);
    IQueryable<T> GetByFilter(IDictionary<string, object> filter);
    void Update(T entity);
    void Delete(T entity);
}
