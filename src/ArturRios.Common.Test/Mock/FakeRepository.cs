using ArturRios.Common.Data;
using ArturRios.Common.Data.Interfaces;

namespace ArturRios.Common.Test.Mock;

public class FakeRepository<T>(Type filterType) : ICrudRepository<T> where T : Entity
{
    private readonly List<T> _items = [];
    private int _nextId;

    public int Create(T entity)
    {
        entity.Id = _nextId++;

        _items.Add(entity);

        return entity.Id;
    }

    public IQueryable<T> GetAll()
    {
        return _items.AsQueryable();
    }

    public T? GetById(int id, bool track = false)
    {
        return _items.FirstOrDefault(item => item.Id == id);
    }

    public void Update(T entity)
    {
        var existingItem = _items.FirstOrDefault(item => item.Id == entity.Id);

        if (existingItem is null)
        {
            throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");
        }

        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            if (!prop.CanWrite || prop.Name == nameof(Entity.Id))
            {
                continue;
            }

            var value = prop.GetValue(entity);
            prop.SetValue(existingItem, value);
        }
    }

    public List<int> Delete(List<int> ids)
    {
        var entities = _items.Where(e => ids.Contains(e.Id)).ToList();

        foreach (var entity in entities)
        {
            _items.Remove(entity);
        }

        return entities.Select(e => e.Id).ToList();
    }
}
