# ICrudRepository

An interface that can be used to implement basic database operations on repository classes

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