# Data Filter

An abstract class to use as a base for classes that contains properties to be used as filters on database queries

[Source code](../src/TechCraftsmen.Core.Data/DataFilter.cs)

## Usage

```csharp
public class CustomFilter : DataFilter
{
    public string? Name { get; set; }
    public DateTime? CreationDate { get; set; }
}
```

The DataFilter class is the type of the parameter passed to the [ICrudRepository](./ICrudRepository.md) method `GetByFilter`. The following approach is recommended:

```csharp
    public IQueryable<CustomEntity> GetByFilter(DataFilter filter)
    {
        if (filter is not CustomFilter customFilter)
        {
            throw new ArgumentException("Invalid filter");
        }
        
        var query = _dbContext.Set<CustomEntity>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(customFilter.Name))
        {
            query = query.Where(e => e.Name == customFilter.Name);
        }

        if (customFilter.CreationDate.HasValue)
        {
            query = query.Where(e => e.Name == customFilter.CreationDate);
        }
        
        return query;
    }
```
