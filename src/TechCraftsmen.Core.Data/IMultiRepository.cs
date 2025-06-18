namespace TechCraftsmen.Core.Data;

public interface IMultiRepository<out T> where T : Entity
{
    IQueryable<T> GetByMultiFilter(DataFilter filter, bool track = false);
    void MultiDelete(IEnumerable<int> ids);
}
