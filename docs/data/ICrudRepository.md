# ICrudRepository

An interface that can be used to implement CRUD (Create, Read, Update, Delete) database operations on repository classes.
It's a generic interface that can be used with any class that represents a database entity. It requires a type parameter that must be a class that inherits from the [Entity](./Entity.md) class.

[Source code](../src/ArturRios.Common.Data/ICrudRepository.cs)

## Usage

```csharp
public class CustomRepository : ICrudRepository<CustomEntity>
{
    public int Create(CustomEntity entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<CustomEntity> GetByFilter(DataFilter filter)
    {
        throw new NotImplementedException();
    }

    public CustomEntity GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(CustomEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}
```
