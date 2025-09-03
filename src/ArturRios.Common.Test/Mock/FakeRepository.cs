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

    public IQueryable<T> GetByFilter(DataFilter filter, bool track = false)
    {
        if (filter.GetType() != filterType)
        {
            throw new ArgumentException($"Filter type must be {filterType.Name}", nameof(filter));
        }

        var filterProperties = filterType.GetProperties();
        IEnumerable<T> query = _items;

        foreach (var prop in filterProperties)
        {
            var filterValue = prop.GetValue(filter);

            if (filterValue is null)
            {
                continue;
            }

            var property = typeof(T).GetProperty(prop.Name);
            if (property is not null)
            {
                query = query.Where(item =>
                {
                    var itemValue = property.GetValue(item);
                    return Equals(itemValue, filterValue);
                });
            }
        }

        return query.AsQueryable();
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

    public void Delete(int id)
    {
        var entity = _items.FirstOrDefault(item => item.Id == id);

        if (entity is null)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        _items.Remove(entity);
    }
}
