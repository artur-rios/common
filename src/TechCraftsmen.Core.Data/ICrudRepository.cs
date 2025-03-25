namespace TechCraftsmen.Core.Data;

public interface ICrudRepository<T> where T : Entity
{
    int Create(T entity);
    IQueryable<T> GetByFilter(DataFilter filter);
    T GetById(int id);
    void Update(T entity);
    void Delete(int id);
}
