// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// Reason: This class is meant to be used in other projects

namespace ArturRios.Common.Data.Interfaces;

public interface ICrudRepository<T> where T : Entity
{
    int Create(T entity);
    T? GetById(int id, bool track = false);
    void Update(T entity);
    List<int> Delete(List<int> ids);
}
